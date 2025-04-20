#  Map Loading Process
Back to [Main read me](../README.md)
 The devloppement process is divided into several stages.
## Iteration Overview

### Todo: Iteration 01
The first iteration focuses on setting up basic UI components and displaying provinces on the map on loading.

- [ ] **Create Adequate Canvas**
  - Set up a Unity Canvas that will serve as the base for UI components.
- [ ] **Display Provinces**
  - Implement a system to visualize the different provinces on the map.

### Todo: Iteration 02
In the second iteration, the goal is to add a 3D terrain, create a sea feature, and overlay the canvas UI on top of a 3D terrain.

- [ ] **Create 3D Terrain**
  - Load 3D terrain to serve as the foundation for the map.
- [ ] **Create Sea**
  - Add an ocean or sea layer to represent water bodies on the map.
- [ ] **Superimpose Canvas and Terrain**
  - Ensure that the UI elements (like province names or info) are correctly overlaid on the 3D terrain.

### Todo: Iteration 03
The third iteration will include an editor tool for managing provinces, as well as visual features for linking province selection to specific colors.

- [ ] **Province Editor Tool**
  - Develop a tool that allows users to create, edit, and manage provinces on the map.
- [ ] **Link Province Selection to Color**
  - Implement functionality that allows users to select a province, which will then be highlighted or assigned a specific color.

## Explanation

The canvas is created based on the size of the `Province_Map.png` image.
Each province is positioned on the canvas using the `lowerX` and `higherY` values, which represent the top-left corner of the sprite. These values are extracted from the map position JSON file (`.Assets/Resources`), which is generated during the map creation phase.

The terrain is created based on `.Assets/Resources/height_map.png` wich is used as a displacement map.
[What is displacement mapping ?](https://www.creativebloq.com/features/a-beginners-guide-to-displacement-and-bump-maps)
