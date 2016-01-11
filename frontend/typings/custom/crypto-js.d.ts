declare module CryptoJS {
    export interface Lib {
        SHA256: any;
        enc: {
            Base64: any;
        }
    }
    
    export var lib: Lib;
}

declare module 'crypto-js' {
    import lib = CryptoJS.lib;
    export = lib;
}