## Dev Map Iteration Overview
Back to [Main read me](../../README.md)
Back to [Road Map](../roadmap.md)

### Todo: Iteration 01
The first iteration focuses on setting up basic UI components and displaying provinces on the map on loading.

- [x] **Create Adequate Canvas**
  - Set up a Unity Canvas that will serve as the base for UI components.
- [x] **Display Provinces**
  - Implement a system to visualize the different provinces on the map.

### Todo: Iteration 02
In the second iteration, the goal is to add a 3D terrain, create a sea feature, and overlay the canvas UI on top of a 3D terrain.

- [X] **Create 3D Terrain**
  - Load 3D terrain to serve as the foundation for the map.
- [X] **Create Sea**
  - Add an ocean or sea layer to represent water bodies on the map.
- [X] **Superimpose Canvas and Terrain**
  - Ensure that the UI elements (like province names or info) are correctly overlaid on the 3D terrain.

### Todo: Iteration 03
The third iteration will include an editor tool for managing provinces, as well as visual features for linking province selection to specific colors.

- [X] **Link Provinces Data**
  - retrive  province data from the json.

- [X] **Auto neighbore system**
  - [X] Make an automated way to logically connect provinces that are next to each others.
  - [X] -Auto province top end of image linked to those at bottom end of image
  - [X] -Auto province left end of image linked to those at right end of image
- [X] **Link Province Selection to Color**
  - Implement functionality that link color to a province id

- [⏳] **Province Editor Tool**
  - [⏳] Develop a tool that allows users to edit province info in game.
  
  - [ ] **Load new map keep province data**
   - [ ] new province will be created only for color that did not appear previously
    - [ ] Delete province object if theire color do not appear in the new map
- [X] **Change provice id plane mat on load**

- [X] **Graph for displacement and distance between provinces**
  - keep track of neighboring provinces
  - add distance

- [X] **stand alona app to keep track of colord used**
- [X] **Save provinces data**

- [X] **prevent camera passing through terrain**

- [⏳] **restrict camera to terrain size**

- [ ] **center camera on terrain on game enter**


### Todo: Iteration 04

- [X] **load data correctly**
- [X] **test projector for provinces**
- [ ] **Add a create country page**



### Todo: Iteration ?

- [ ] **terrain height addapting to terrain type**
- [ ] **province highlight on hover**