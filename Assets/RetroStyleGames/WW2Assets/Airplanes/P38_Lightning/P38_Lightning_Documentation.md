# Lockheed P-38 Lightning – Documentation

## Overview
This package includes a high-quality **Lockheed P-38 Lightning** WW2 airplane model designed for real-time applications such as games, cinematics, and simulations.

The model comes with **4 historical skin variations**, each provided as a ready-to-use material and prefab, so you can easily match different squadrons, theatres, and storytelling needs.

---

## Render Pipeline Compatibility

This asset is fully compatible with:

- Built-in Render Pipeline (BRP)
- Universal Render Pipeline (URP)
- High Definition Render Pipeline (HDRP)

All materials use custom **RSG Shader Graphs** that support all three render pipelines, so a single material set works everywhere — no material swapping required.

---

## Technical Details

- **Vertices:** 18,543
- **Triangles:** 22,124
- **Meshes:** 1 skinned mesh (2 material slots)
- **Materials:** 5 (4 skin + 1 transparent glass, RSG_Lit_Specular Shader Graph, specular workflow)
- **Textures:** 9 (Base Color, Specular Smoothness, Normal)
- **Texture Resolution:** 2048 × 2048 px
- **Skins:** 4 (Skin 01, Skin 02, Skin 03, Skin 04)
- **Rigged:** Yes — Generic rig, 14 bones
- **Animations:** No
- **UV Mapping:** Yes
- **LOD:** No

---

## Model

- **File:** SK_RSG_Plane_P38_Lightning.fbx
- **Skinned mesh:** SK_RSG_Plane_P38_Lightning
- **Rig type:** Generic (14 bones)

| Slot | Part | Material (Skin 01) | Vertices | Triangles |
|---|---|---|---|---|
| 0 | Body | M_RSG_Plane_P38_Lightning_Skin01 | 18,265 | 21,672 |
| 1 | Canopy glass | M_RSG_Plane_P38_Lightning_Glass | 278 | 452 |
| **Total** | | | **18,543** | **22,124** |

### Bone Hierarchy

- `root`
  - `cabin`
    - `chassis_dec_01_front`
    - `chassis_dec_01_l`
    - `chassis_dec_01_r`
    - `chassis_dec_02_l`
    - `chassis_dec_02_r`
    - `chassis_front`
      - `wheel_mid`
    - `chassis_l`
      - `wheel_l`
    - `chassis_r`
      - `wheel_r`
    - `propeller_l`
    - `propeller_r`

---

## Textures

9 textures at 2048 × 2048 px:

- **Per skin:** Base Color (BC), Specular Smoothness (SS)  
- **Shared:** 1 Normal map (N)  

Specular Smoothness maps pack the specular color in RGB and smoothness in the alpha channel. Normal map is shared across all skins.

---

## Skins

4 skin variations, each with its own material and prefab:

- **Skin 01** – P_RSG_Plane_P38_Lightning_Skin01
- **Skin 02** – P_RSG_Plane_P38_Lightning_Skin02
- **Skin 03** – P_RSG_Plane_P38_Lightning_Skin03
- **Skin 04** – P_RSG_Plane_P38_Lightning_Skin04

---

## Features

- Detailed WW2-era design
- 4 historical skin variations
- Rigged skeletal mesh — animatable canopy, propeller, landing-gear and wheel bones
- Optimized topology for real-time performance
- Fully UV-mapped model
- Cross-pipeline support (BRP, URP, HDRP) via custom Shader Graphs
- Ready-to-use prefab per skin
- Demo scene per render pipeline (BRP, URP, HDRP) that automatically lines up every WW2 Assets airplane and pilot installed in the project

---

## Included Content

- 1 Airplane model – SK_RSG_Plane_P38_Lightning.fbx (skeletal mesh, 2 material slots, 14 bones)
- 5 Materials (4 skin + 1 transparent glass, RSG_Lit_Specular Shader Graph)
- 9 Textures (2048 × 2048 px)
- 4 Prefabs with configured materials
- 3 Demo scenes – WW2_Demo_BRP, WW2_Demo_URP, WW2_Demo_HDRP (shared **WW2Assets/Demo** folder with the populator script, a ground material and URP/HDRP volume profiles)

---

## Demo Scene

The shared **WW2Assets/Demo** folder ships one demo scene per render pipeline: **WW2_Demo_BRP**, **WW2_Demo_URP** and **WW2_Demo_HDRP** (in `Demo/Scenes`). Open the one that matches your project's render pipeline.

The scene itself contains no airplane or pilot. When it opens, the **WW2 Demo** object (`WW2DemoPopulator`, the only script in the package) finds every RetroStyleGames WW2 prefab in the project and lines it up: one row per aircraft with all of its skins side by side, smallest aircraft in front, and the pilots in a row before them. Only the packs you have imported appear, and importing another WW2 Assets pack adds its content the next time the scene loads — nothing has to be configured. The spawned objects are never saved into the scene; right-click the component for **Populate** and **Clear**, and adjust the row and aircraft gaps in the Inspector. Besides the three scenes, the folder holds only the populator script, a flat ground material (RSG_Lit) and the URP/HDRP volume profiles; the whole **Demo** folder can be deleted without affecting the models, materials or prefabs.

---

## Setup Instructions

1. Import the package into your Unity project.
2. Ensure your project is using one of the supported render pipelines (BRP, URP, or HDRP).
3. Optional: open the demo scene matching your pipeline (**WW2Assets/Demo/Scenes/WW2_Demo_BRP**, **_URP** or **_HDRP**) to see every installed WW2 Assets airplane and pilot lined up.
4. Drag and drop the desired skin prefab (e.g. **P_RSG_Plane_P38_Lightning_Skin01**) into your scene.

---

## Notes

- The model is a rigged skeletal mesh (Generic rig). No animation clips are included — animate the `cabin`, propeller, landing-gear (`chassis_*`) and wheel bones directly.
- No LODs are provided.
- Canopy glass uses its own **transparent** material (`M_RSG_Plane_P38_Lightning_Glass`), shared by every skin and carrying no textures. The painted skin sheets cover the airframe only.

---

## Support

If you have any questions or need assistance, please contact support via the Unity Asset Store page.

---

## License

This asset is provided under the standard Unity Asset Store license agreement.
