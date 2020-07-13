# 卡通水材质球

## 名称

卡通水材质球

## Tags

水，卡通，潮汐，香肠派对

## 预览效果

<p align="center">
  <img src="./imgs/water_preview01.gif"
  width="400"/>
</p>

## 渲染管线版本

LWRP_4.9.0

## 引用 Shader

[shd_water_v1.shader](./shd_water_v1.shader)

## 材质面板参数说明

<p align="center">
<img src="./imgs/water_parameters.png"
width="400">
</p>

- Total Scale 整体对所有的纹理进行缩放，方便快速匹配不同大小的水体模型
- [Gradient](./Textures/tex_ramp_maincolor.png) 作为主颜色过度的采用，该过度图的 Alpha 通道也会影响水面的透明过度情况
- Color Gradient Distance 改变其参数会影响水面与岸边在颜色上的过度情况，数值越大过度越宽
- Foam Color 普通物体的泡沫颜色
- PlayerFoamColor 角色的泡沫颜色
- [Foam Main Texture](./Textures/tex_mainfoam_01.png) 水岸泡沫主纹理贴图 (Tiling X 可以控制主纹理在 Y 方向的 Tiling 程度，Tiling Y 无效)
- [Foam Detil Texture](./Textures/tex_detailfoam_01.png) 水岸泡沫细节纹理贴图 (Tiling X 可以控制细节纹理在 Y 方向的 Tiling 程度，Tiling Y 无效。该贴图会受 Noise Texture 影响，会溶解出现，漂浮一段时间，然后溶解消失)
- [Noise Texture](./Textures/tex_Noise_anisotropic2.png) 噪波纹理图，会影响泡沫细节的溶解形态
- Dissolve Range 影响溶解时间，值越大溶解范围越小
- Foam Move Distance 普通物体的泡沫移动的范围，数值越大，泡沫位移幅度越大 (同时会影响泡沫的宽度，需要和 FoamWidth 对应设置)
- PlayerFoamMoveDistance 玩家的泡沫移动的范围
- FoamWidth 普通物体的泡沫宽度
- PlayerFoamWidth 玩家泡沫宽度
- [Caustics Texture](./Textures/tex_Caustics_01.png) 水岸焦散线贴图
- [Normal Texture](./Textures/tex_water_noise.jpg) 法线贴图
- Normal Scale 法线缩放
- Fresnel 菲尼尔系数，影响反射天空盒的范围
- Shininess  高光系数
- Specular Color 高光颜色
- Wind Speed 水体流动速度（法线贴图的流动速度）
- Wave Speed 泡沫冲刷的一个周期的速度
- Shake Step 泡沫位移时候产生的晃动的距离
- Shake Speed 晃动位移的速度
- [Reflection Texture](./Textures/tex_skymap_rainbow.png) 反射天空盒贴空 (CubeMap)
- Blend Color 海岸与水体过度颜色 (需要和天空盒设置为同一种颜色)
- Near 过度颜色近处的羽化值
- Far 过度颜色产生的位置
- CullColor 效果裁剪最终形态的主颜色

## 其他补充说明

这个一个卡通水渲染材质球，会根据摄像机距离进行效果的动态裁剪。所以使用要保证场景中有渲染的 Camera 并在水面上挂载 [WaterCull](./script/WaterCull.cs) 脚本。

[详细说明文档](https://www.dropbox.com/scl/fi/ah7eqhobfwno4qjnnsk1a/.paper?dl=0&rlkey=h4utu2yc54iczzkoqpqdwo4t0)