function initializeTracking(wsSecret) {
    // 1. Inject Fullscreen CSS dynamically so you don't need a separate stylesheet
    if (!document.getElementById('tracking-fullscreen-styles')) {
        const style = document.createElement('style');
        style.id = 'tracking-fullscreen-styles';
        style.innerHTML = `
            .visitor-fullscreen {
                position: fixed !important;
                top: 0 !important;
                left: 0 !important;
                width: 100vw !important;
                height: 100vh !important;
                z-index: 1050 !important; /* Sits above Bootstrap navbars */
                margin: 0 !important;
                border-radius: 0 !important;
                background: #000 !important;
            }
            .visitor-fullscreen .card-body {
                aspect-ratio: auto !important; /* Allow the body to fill the screen */
                height: 100% !important;
            }
        `;
        document.head.appendChild(style);
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/myhub")
        .withAutomaticReconnect()
        .build();

    connection.start()
        .then(() => {
            console.log("Connected to SignalR Tracking Hub");
        })
        .catch(err => console.error(err));

    const activeVisitors = [];
    const container = document.getElementById("live-visitors-container");

    connection.on(`ReceiveFromServer-${wsSecret}`, (data) => {
        // HANDLE HTML INJECTION (If the payload contains the DOM)
        if (typeof data === "string" && data.includes("<body>")) {
            const splitData = data.split("|");
            let theInnerHTML = splitData[splitData.length - 1];
            const theVisitorId = splitData[splitData.length - 2];

            const iframe = document.getElementById(`iframe-${theVisitorId}`);

            if (iframe) {
                theInnerHTML = theInnerHTML.replace(/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, '');

                iframe.srcdoc = theInnerHTML;

                if (iframe.srcdoc == "") {
                    const pathSegments = window.location.pathname.split('/');
                    const currentWebsiteId = pathSegments[pathSegments.length-1];

                    let trackingData = {
                        ConnectionId: theVisitorId,
                        WebsiteId: currentWebsiteId
                    }

                    let baseUrl = window.location.origin;

                    fetch(`${baseUrl}/Visitors/Create`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(trackingData)
                    }).catch(err => console.error("WebTrack start failed:", err));
                }
            }

            return;
        }

        // HANDLE CURSOR & METADATA
        try {
            const parsedData = JSON.parse(data);
            // Debug
            //console.log(parsedData);

            if (!activeVisitors.includes(parsedData.visitorId)) {
                activeVisitors.push(parsedData.visitorId);

                const col = document.createElement("div");
                col.className = "col-12 col-lg-6 col-xl-4 mb-4";
                col.id = `visitor-card-${parsedData.visitorId}`;

                col.innerHTML = `
                    <div class="card shadow-sm border-0 rounded-3 overflow-hidden h-100 bg-dark visitor-card" style="cursor: pointer; transition: transform 0.2s ease-in-out;" title="Click to expand/collapse">
                        <div class="card-header bg-secondary bg-opacity-25 border-0 d-flex justify-content-between align-items-center py-2">
                            <span class="fw-bold text-white small"><i class="bi bi-display text-primary me-2"></i>Visitor ${parsedData.visitorId.substring(0, 5)}</span>
                            <span class="badge bg-danger text-white rounded-pill px-2">
                                <span class="spinner-grow spinner-grow-sm me-1" style="width: 8px; height: 8px;"></span>Live
                            </span>
                        </div>
                        
                        <div class="card-body p-0 position-relative" style="aspect-ratio: 16/9; overflow: hidden;" id="screen-wrapper-${parsedData.visitorId}">
                            
                            <div id="scale-container-${parsedData.visitorId}" style="width: 1920px; height: 1080px; transform-origin: top left; position: absolute; top: 0; left: 0;">
                                
                                <iframe id="iframe-${parsedData.visitorId}" style="width: 100%; height: 100%; border: none; pointer-events: none; background: white;"></iframe>

                                <div id="cursor-${parsedData.visitorId}" style="display: none; position: absolute; z-index: 10; top: 0; left: 0; pointer-events: none; transition: transform 0.05s linear;">
                                    <svg width="24" height="36" viewBox="0 0 24 36" fill="none" xmlns="http://www.w3.org/2000/svg" style="filter: drop-shadow(2px 2px 4px rgba(0,0,0,0.6));">
                                        <path d="M1 1L10.5 34.5L14.5 20.5L23 16.5L1 1Z" fill="#ff0000" stroke="white" stroke-width="2" stroke-linejoin="round"/>
                                    </svg>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                container.appendChild(col);

                const wrapper = document.getElementById(`screen-wrapper-${parsedData.visitorId}`);
                const scaler = document.getElementById(`scale-container-${parsedData.visitorId}`);
                const cardElement = col.querySelector('.visitor-card');

                // Advanced scaling algorithm ensuring it fits tall OR wide monitors
                const updateScale = () => {
                    const widthRatio = wrapper.clientWidth / 1920;
                    const heightRatio = wrapper.clientHeight / 1080;

                    // Math.min forces the 1920x1080 box to fit completely inside the wrapper without cropping
                    const scaleFactor = Math.min(widthRatio, heightRatio);
                    scaler.style.transform = `scale(${scaleFactor})`;
                };

                // CLICK EVENT: Toggle Fullscreen overlay
                cardElement.addEventListener('click', () => {
                    cardElement.classList.toggle('visitor-fullscreen');

                    // Lock the main page's scrollbar so we don't get double scrolling
                    if (cardElement.classList.contains('visitor-fullscreen')) {
                        document.body.style.overflow = 'hidden';
                    } else {
                        document.body.style.overflow = '';
                    }

                    // Run the scaler immediately after the layout shift
                    setTimeout(updateScale, 50);
                });

                // Initialize scale and listen for window resizes
                updateScale();
                window.addEventListener('resize', updateScale);
            }

            // UPDATE CURSOR POSITION
            if (parsedData.percentX !== undefined && parsedData.percentY !== undefined) {
                const cursorElement = document.getElementById(`cursor-${parsedData.visitorId}`);
                const scalerElement = document.getElementById(`scale-container-${parsedData.visitorId}`);
                const wrapperElement = document.getElementById(`screen-wrapper-${parsedData.visitorId}`);

                if (cursorElement && scalerElement && wrapperElement) {
                    if (cursorElement.style.display === "none") {
                        cursorElement.style.display = "block";
                    }

                    // Resize the iframe dynamically to match the visitor's exact screen size.
                    // This ensures mobile visitors look like mobile, and desktop looks like desktop!
                    if (parsedData.visitorWidth && parsedData.visitorHeight) {
                        scalerElement.style.width = `${parsedData.visitorWidth}px`;
                        scalerElement.style.height = `${parsedData.visitorHeight}px`;

                        // Instantly recalculate the wrapper scaling so it doesn't clip out of bounds
                        const widthRatio = wrapperElement.clientWidth / parsedData.visitorWidth;
                        const heightRatio = wrapperElement.clientHeight / parsedData.visitorHeight;
                        const scaleFactor = Math.min(widthRatio, heightRatio);
                        scalerElement.style.transform = `scale(${scaleFactor})`;
                    }

                    // Apply the cursor based on the percentages of the container's dimensions
                    const targetX = scalerElement.clientWidth * parsedData.percentX;
                    const targetY = scalerElement.clientHeight * parsedData.percentY;

                    cursorElement.style.transform = `translate(${targetX}px, ${targetY}px)`;
                }
            }

            // DRAW RED CLICK RIPPLE
            if (parsedData.type === "mousedown" || parsedData.type === "click") {
                const container = document.getElementById(`scale-container-${parsedData.visitorId}`);

                if (container && parsedData.percentX !== undefined && parsedData.percentY !== undefined) {
                    // Create the circle
                    const ripple = document.createElement('div');
                    ripple.style.position = 'absolute';
                    ripple.style.width = '20px';
                    ripple.style.height = '20px';
                    ripple.style.background = 'rgba(255, 0, 0, 0.6)'; // Red with some transparency
                    ripple.style.borderRadius = '50%';
                    ripple.style.pointerEvents = 'none'; // So it doesn't block your own clicks
                    ripple.style.zIndex = '9999'; // Force it to the top

                    // Calculate exact pixel position using the percentages
                    const targetX = container.clientWidth * parsedData.percentX;
                    const targetY = container.clientHeight * parsedData.percentY;

                    // Center the 20x20px circle precisely on the click coordinates (-10px offset)
                    ripple.style.left = `${targetX - 10}px`;
                    ripple.style.top = `${targetY - 10}px`;

                    // Start tiny
                    ripple.style.transform = 'scale(0)';
                    ripple.style.transition = 'transform 0.4s ease-out, opacity 0.4s ease-out';

                    container.appendChild(ripple);

                    // Animate it expanding and fading out
                    requestAnimationFrame(() => {
                        ripple.style.transform = 'scale(3)';
                        ripple.style.opacity = '0';
                    });

                    // Remove it from the DOM after the animation finishes
                    setTimeout(() => ripple.remove(), 400);
                }
            }

        } catch (e) {
            console.error("Failed to parse SignalR JSON data", e);
        }
    });
}

// Toggle Secret Eye Logic
document.getElementById('toggleSecretBtn').addEventListener('click', function () {
    const secretInput = document.getElementById('wsSecretInput');
    const icon = document.getElementById('toggleSecretIcon');

    if (secretInput.type === 'password') {
        secretInput.type = 'text';
        icon.classList.remove('bi-eye');
        icon.classList.add('bi-eye-slash');
    } else {
        secretInput.type = 'password';
        icon.classList.remove('bi-eye-slash');
        icon.classList.add('bi-eye');
    }
});