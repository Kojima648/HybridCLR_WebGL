最近对WebGL相关的话题变得热火，因为HybridCLR支持了WebGL平台，使得微信/抖音的小游戏也可以做到热更新 鉴于群里老有人问WebGL打包和配置的相关问题，特此出一期演示视频，在原演示项目上做了修改，使得可以直接跑或作为一个开始。

 Demo说明如下： 

1、il2cpp必须要全局安装 

2、演示demo中的besthttp插件可以剔除 

3、未接入资源管理相关的东西，可以按照自己的需求接 

4、现在加载都用的unitywebrequest, 直接从streamingasset加载ab包 

5、加载顺序为先加载元数据再加载热更新，然后加载prefab 最后加载的besthttp的演示场景 

6、demo只是提供一个思路，可能没有实质性帮助，后续或许不会再更新