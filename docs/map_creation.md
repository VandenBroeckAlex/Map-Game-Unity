# Map Creation Process
Back to [Main read me](../README.md)
[More about the map creation code ](./map_creation_code.md)

## Overview

The `SpriteCreator` is designed to split a single image into multiple smaller sprites based on the colors present in the image. Each color in the image is treated as a distinct `province`, and the script will generate PNG files for each unique `province`, saving them in `Assets/sprites_terrain/provinces_split`(It will more than probably cnage). Additionally, it creates a JSON file that holds information about each sprite's position and color.


### Usage

1. Place your base image (the name must be `Province_Map.png`) in the `Resources` folder.
2. The game will automatically generate the `Provinces` sprites and save them in the `Assets/sprites_terrain/provinces_split` directory.
4. The `map_position.json` file will be saved in the `Resources` folder, containing all the sprite data.

### Notes

- The process is run only if the reload map button is toogle / if the sprite folder is empty or if the map_position JSonis empty or non existant

- The `Province_Map.png` should not contain pure black as it might interfere with sprite creation.

- When creating your `Province_Map.png` make sure to have `hard edges` as the slightest change of color will be considered as a new `Province`.


The edge should look like this. ![Yes](./img/good_edge.png) 
The edge should `not` look like this ![No](./img/bad_edge.png) 
