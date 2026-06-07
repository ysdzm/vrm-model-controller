# vrm-model-controller

Unity package for controlling VRM 1.0 models.

## Install

This package uses UniVRM for VRM 1.0 support.

In Unity, open `Window > Package Manager`, click `+`, select `Add package from git URL...`, and add these packages.

```text
https://github.com/vrm-c/UniVRM.git?path=/Packages/UniGLTF#v0.131.0
https://github.com/vrm-c/UniVRM.git?path=/Packages/VRM10#v0.131.0
https://github.com/ysdzm/vrm-model-controller.git?path=/Packages/com.ysdzm.vrm-model-controller
```

Or edit your Unity project's `Packages/manifest.json`.

```json
{
  "dependencies": {
    "com.vrmc.gltf": "https://github.com/vrm-c/UniVRM.git?path=/Packages/UniGLTF#v0.131.0",
    "com.vrmc.vrm": "https://github.com/vrm-c/UniVRM.git?path=/Packages/VRM10#v0.131.0",
    "com.ysdzm.vrm-model-controller": "https://github.com/ysdzm/vrm-model-controller.git?path=/Packages/com.ysdzm.vrm-model-controller"
  }
}
```

`com.vrmc.univrm` is for VRM 0.x, so it is not required for VRM 1.0.

## Usage

1. Import or place a VRM 1.0 model in your scene.
2. Select the VRM root GameObject.
3. Add `VrmModelController` from the Inspector.
