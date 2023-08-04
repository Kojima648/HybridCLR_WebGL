
const unityNamespace = {
    canvas: GameGlobal.canvas,
    
    canvas_width: GameGlobal.canvas.width,
    
    canvas_height: GameGlobal.canvas.height,
    navigator: GameGlobal.navigator,
    XMLHttpRequest: GameGlobal.XMLHttpRequest,
    
    hideTimeLogModal: true,
    
    enableDebugLog: false,
    
    bundleHashLength: 32,
    
    releaseMemorySize: 31457280,
    unityVersion: '2021.3.3f1',
    
    unityColorSpace: 'Gamma',
    convertPluginVersion: '202308031943',
    
    streamingUrlPrefixPath: '',
    // DATA_CDN + dataFileSubPrefix + datafilename
    dataFileSubPrefix: '',
    
    maxStorage: 200,
    
    texturesHashLength: 8,
    
    texturesPath: 'Assets/Textures',
    
    needCacheTextures: true,
    
    ttlAssetBundle: 5,
    
    enableProfileStats: false,
    
    preloadWXFont: false,
    
    iOSAutoGCInterval: 10000,
    
    usedTextureCompression: GameGlobal.USED_TEXTURE_COMPRESSION,
    
    usedAutoStreaming: false,
};

unityNamespace.monitorConfig = {
    
    showSuggestModal: false,
    
    enableMonitor: true,
    
    fps: 10,
    
    showResultAfterLaunch: true,
    
    monitorDuration: 30000,
};

unityNamespace.isCacheableFile = function (path) {
    
    const cacheableFileIdentifier = ["StreamingAssets"];
    
    const excludeFileIdentifier = ["json"];
    if (cacheableFileIdentifier.some(identifier => path.includes(identifier)
        && excludeFileIdentifier.every(excludeIdentifier => !path.includes(excludeIdentifier)))) {
        return true;
    }
    return false;
};

unityNamespace.isWXAssetBundle = function (path) {
    return unityNamespace.WXAssetBundles.has(unityNamespace.PathInFileOS(path));
};
unityNamespace.PathInFileOS = function (path) {
    return path.replace(`${wx.env.USER_DATA_PATH}/__GAME_FILE_CACHE`, '');
};
unityNamespace.WXAssetBundles = new Map();
// 清理缓存时是否可被自动清理；返回true可自动清理；返回false不可自动清理
unityNamespace.isErasableFile = function (info) {
    // 用于特定AssetBundle的缓存保持
    if (unityNamespace.WXAssetBundles.has(info.path)) {
        return false;
    }
    // 达到缓存上限时，不会被自动清理的文件
    const inErasableIdentifier = [];
    if (inErasableIdentifier.some(identifier => info.path.includes(identifier))) {
        return false;
    }
    return true;
};
const { version, SDKVersion, platform, renderer, system } = wx.getSystemInfoSync();
unityNamespace.version = version;
unityNamespace.SDKVersion = SDKVersion;
unityNamespace.platform = platform;
unityNamespace.renderer = renderer;
unityNamespace.system = system;
unityNamespace.isPc = platform === 'windows' || platform === 'mac';
unityNamespace.isDevtools = platform === 'devtools';
unityNamespace.isMobile = !unityNamespace.isPc && !unityNamespace.isDevtools;
unityNamespace.isH5Renderer = unityNamespace.isMobile && unityNamespace.renderer === 'h5';
unityNamespace.isIOS = platform === 'ios';
unityNamespace.isAndroid = platform === 'android';
GameGlobal.WebAssembly = GameGlobal.WXWebAssembly;
GameGlobal.unityNamespace = GameGlobal.unityNamespace || unityNamespace;
GameGlobal.realtimeLogManager = wx.getRealtimeLogManager();
GameGlobal.logmanager = wx.getLogManager({ level: 0 });
GameGlobal.onCrash = GameGlobal.unityNamespace.onCrash = function () {
    GameGlobal.manager.showAbort();
    const sysInfo = wx.getSystemInfoSync();
    wx.createFeedbackButton({
        type: 'text',
        text: '提交反馈',
        style: {
            left: (sysInfo.screenWidth - 184) / 2,
            top: sysInfo.screenHeight / 3 + 140,
            width: 184,
            height: 40,
            lineHeight: 40,
            backgroundColor: '#07C160',
            color: '#ffffff',
            textAlign: 'center',
            fontSize: 16,
            borderRadius: 4,
        },
    });
};
export default GameGlobal.unityNamespace;
