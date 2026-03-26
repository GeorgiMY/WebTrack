type VisitorInformation = {
    backend: {
        network: {
            ipAddress: string | null;
            port: number | null;
            protocol: string | null; // "HTTP/1.1", "HTTP/2", "HTTP/3"
            tls?: {
                version: string | null;
                cipher: string | null;
                serverNameIndication: string | null;
                alpn: string | null;
                clientCertificatePresented: boolean | null;
                clientCertificate?: {
                    subject: string | null;
                    issuer: string | null;
                    serialNumber: string | null;
                    validFrom: string | null;
                    validTo: string | null;
                    fingerprint: string | null;
                } | null;
            } | null;
        };

        request: {
            method: string;
            scheme: string | null; // "http" | "https"
            host: string | null;
            hostname: string | null;
            port: number | null;
            path: string;
            queryString: string | null;
            queryParameters: Record<string, string | string[]>;
            fragment: null; // URL fragment is not sent to the server
            headers: Record<string, string | string[]>;
            rawHeaders?: Array<{ name: string; value: string }>;
            body: string | ArrayBuffer | Uint8Array | null;
            contentLength: number | null;
            contentType: string | null;
            referrer: string | null;
            origin: string | null;
            authorization: string | null;
            cookies: Record<string, string>;
        };

        clientHints: {
            userAgent: string | null;
            secChUa: string | null;
            secChUaMobile: string | null;
            secChUaPlatform: string | null;
            secChUaPlatformVersion: string | null;
            secChUaArch: string | null;
            secChUaBitness: string | null;
            secChUaModel: string | null;
            secChUaFullVersion: string | null;
            secChUaFullVersionList: string | null;
            secChPrefersColorScheme: string | null;
            secChPrefersReducedMotion: string | null;
            secChPrefersReducedTransparency: string | null;
            secChViewportWidth: string | null;
            secChWidth: string | null;
            secChDpr: string | null;
            secChDeviceMemory: string | null;
            secChRtt: string | null;
            secChDownlink: string | null;
            secChEct: string | null;
            secChSaveData: string | null;
            secChUaWow64: string | null;
            secChUaFormFactors: string | null;
        };

        fetchMetadata: {
            secFetchSite: string | null;
            secFetchMode: string | null;
            secFetchUser: string | null;
            secFetchDest: string | null;
        };

        localization: {
            acceptLanguage: string | null;
            acceptCharset: string | null;
            acceptEncoding: string | null;
        };

        connection: {
            forwarded: string | null;
            xForwardedFor: string | null;
            xForwardedProto: string | null;
            xForwardedHost: string | null;
            via: string | null;
            cfConnectingIp?: string | null;
            trueClientIp?: string | null;
            fastlyClientIp?: string | null;
        };

        upload: {
            hasFiles: boolean;
            files?: Array<{
                fieldName: string;
                originalFileName: string;
                mimeType: string | null;
                size: number;
            }>;
        };
    };

    frontend: {
        location: {
            href: string;
            origin: string;
            protocol: string;
            host: string;
            hostname: string;
            port: string;
            pathname: string;
            search: string;
            hash: string;
        };

        document: {
            referrer: string;
            title: string;
            characterSet: string;
            contentType: string;
            readyState: string;
            visibilityState: string;
            lastModified: string;
            cookie: string;
        };

        navigator: {
            userAgent: string;
            platform: string;
            language: string;
            languages: string[];
            cookieEnabled: boolean;
            onLine: boolean;
            hardwareConcurrency: number | null;
            deviceMemory?: number | null;
            maxTouchPoints: number;
            webdriver?: boolean;
            doNotTrack: string | null;
            pdfViewerEnabled?: boolean | null;
            vendor?: string;
            product?: string;
            productSub?: string;
            appName?: string;
            appVersion?: string;
            appCodeName?: string;
            oscpu?: string | null;
            vendorSub?: string | null;
            userAgentData?: {
                brands?: Array<{ brand: string; version: string }>;
                mobile?: boolean;
                platform?: string;
            } | null;
        };

        screen: {
            width: number;
            height: number;
            availWidth: number;
            availHeight: number;
            colorDepth: number;
            pixelDepth: number;
            orientation?: {
                type: string;
                angle: number;
            } | null;
        };

        viewport: {
            innerWidth: number;
            innerHeight: number;
            outerWidth: number;
            outerHeight: number;
            devicePixelRatio: number;
            scrollX: number;
            scrollY: number;
            pageXOffset: number;
            pageYOffset: number;
        };

        timezone: {
            timeZone: string | null;
            offsetMinutes: number;
            localeResolved?: string | null;
        };

        internationalization: {
            locale: string | null;
            calendars?: string[];
            numberingSystems?: string[];
        };

        storage: {
            cookiesEnabled: boolean;
            localStorageAvailable: boolean;
            sessionStorageAvailable: boolean;
            indexedDbAvailable: boolean;
        };

        permissions?: {
            geolocation?: PermissionState | null;
            notifications?: PermissionState | null;
            camera?: PermissionState | null;
            microphone?: PermissionState | null;
            clipboardRead?: PermissionState | null;
            clipboardWrite?: PermissionState | null;
            persistentStorage?: PermissionState | null;
            midi?: PermissionState | null;
            backgroundSync?: PermissionState | null;
        };

        media: {
            prefersColorScheme: "light" | "dark" | "no-preference" | null;
            prefersReducedMotion: "reduce" | "no-preference" | null;
            prefersReducedTransparency?: "reduce" | "no-preference" | null;
            prefersContrast?: "more" | "less" | "custom" | "no-preference" | null;
            hover: boolean | null;
            anyHover: boolean | null;
            pointer: "none" | "coarse" | "fine" | null;
            anyPointer: Array<"none" | "coarse" | "fine"> | null;
            colorGamut?: "srgb" | "p3" | "rec2020" | null;
            monochrome?: boolean | null;
            invertedColors?: boolean | null;
            forcedColors?: boolean | null;
        };

        pageLifecycle: {
            performanceTimeOrigin: number | null;
            navigationType?: string | null;
            activatedAt?: number | null;
            wasDiscarded?: boolean | null;
        };

        performance: {
            now: number | null;
            timing?: {
                navigationStart?: number;
                unloadEventStart?: number;
                unloadEventEnd?: number;
                redirectStart?: number;
                redirectEnd?: number;
                fetchStart?: number;
                domainLookupStart?: number;
                domainLookupEnd?: number;
                connectStart?: number;
                secureConnectionStart?: number;
                connectEnd?: number;
                requestStart?: number;
                responseStart?: number;
                responseEnd?: number;
                domLoading?: number;
                domInteractive?: number;
                domContentLoadedEventStart?: number;
                domContentLoadedEventEnd?: number;
                domComplete?: number;
                loadEventStart?: number;
                loadEventEnd?: number;
            } | null;
            navigationEntries?: Array<{
                name: string;
                entryType: string;
                startTime: number;
                duration: number;
                initiatorType?: string;
                nextHopProtocol?: string;
                renderBlockingStatus?: string;
                workerStart?: number;
                redirectStart?: number;
                redirectEnd?: number;
                fetchStart?: number;
                domainLookupStart?: number;
                domainLookupEnd?: number;
                connectStart?: number;
                secureConnectionStart?: number;
                connectEnd?: number;
                requestStart?: number;
                responseStart?: number;
                responseEnd?: number;
                transferSize?: number;
                encodedBodySize?: number;
                decodedBodySize?: number;
            }>;
        };

        geolocation?: {
            latitude: number;
            longitude: number;
            altitude: number | null;
            accuracy: number | null;
            altitudeAccuracy: number | null;
            heading: number | null;
            speed: number | null;
            timestamp: number;
        } | null;

        inputCapabilities?: {
            touchSupported: boolean | null;
            pointerEventsSupported: boolean | null;
        };

        windowContext: {
            isSecureContext: boolean;
            crossOriginIsolated?: boolean;
            devicePixelRatio: number;
            openerExists: boolean;
            historyLength: number;
        };
    };

    interaction: {
        timestamp: string;
        pageLoadTime: number | null;
        initialUrl: string | null;
        landingPage: string | null;
        referrer: string | null;
        events?: Array<{
            type: string;
            timestamp: number;
            target?: string | null;
            x?: number | null;
            y?: number | null;
            key?: string | null;
            value?: string | null;
        }>;
    };

    identifiers: {
        requestId?: string | null;
        sessionId?: string | null;
        visitorId?: string | null;
        csrfToken?: string | null;
    };

    raw: {
        serverReceivedAt: string;
        clientCollectedAt?: string | null;
    };
};