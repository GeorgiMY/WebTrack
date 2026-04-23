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
            const theInnerHTML = splitData[splitData.length - 1];
            const theVisitorId = splitData[splitData.length - 2];

            const iframe = document.getElementById(`iframe-${theVisitorId}`);

            if (iframe && iframe.srcdoc == "") {
                iframe.srcdoc = theInnerHTML;

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

            return;
        }

        // HANDLE CURSOR & METADATA
        try {
            const parsedData = JSON.parse(data);

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
            if (parsedData.x !== undefined && parsedData.y !== undefined) {
                const cursorElement = document.getElementById(`cursor-${parsedData.visitorId}`);
                if (cursorElement) {
                    if (cursorElement.style.display === "none") {
                        cursorElement.style.display = "block";
                    }
                    cursorElement.style.transform = `translate(${parsedData.x}px, ${parsedData.y}px)`;
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