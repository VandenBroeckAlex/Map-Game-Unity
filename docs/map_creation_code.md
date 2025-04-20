# Script Breakdown
Back to [Map creation process](./map_creation.md)
## Key Features

- **Image Processing:** It reads an image and processes the pixels.
- **Sprite Generation:** It generates individual sprites based on unique pixel colors.
- **JSON Output:** A JSON file is created with metadata about each sprite (such as color and position).
- **Asset Handling:** The script manages sprites and texture settings in Unity, ensuring they are correctly imported and set to the Sprite texture type.


## Variables

- **pathSave:** The directory where the generated sprites will be saved.
- **givenId:** A counter for generating unique IDs for sprites.
- **spriteList:** A list of `SpriteObj` objects, each representing a sprite.
- **spriteListJSON:** A list of `SpriteObjJSON` objects used for JSON export.
- **BaseImg:** The Province_Map.png image (`Texture2D`) that will be split into sprites.

## Classes

### SpriteObj
This class represents a single sprite:

- **spriteColor:** The color of the sprite.
- **spritePixels:** The list of pixels (positions) that make up this sprite.(Once the image created this variable is drop)
- **higherX, higherY, lowerX, lowerY:** Coordinates that define the bounding box of the sprite.
- **id:** A unique identifier for the sprite.

### SpriteObjJSON
This is a serializable version of `SpriteObj` used for JSON export:

- **id:** The unique identifier for the sprite.
- **spriteColor:** The RGB color of the sprite (as a float array).
- **lowerX, higherY:** Coordinates that define the sprite's position in the image.

### CombinedJSON
This class contains the entire sprite data structure, including the canvas size and the list of sprites:

- **canvaWidth, canvaHeight:** The dimensions of the base image.
- **spriteListJSON:** The list of `SpriteObjJSON` objects.

## Methods

### `Awake()`
The `Awake` method is automatically called when the script is initialized. Checks whether the map needs recalculating, and triggers the sprite creation process.

### `DeleteOldSprite()`
Deletes any existing sprite files from the `sprites_terrain/provinces_split` directory to ensure the directory is clean before creating new sprites.

### `GenerateMapSprite()`
This is the core function that processes the image:

- It loops over each pixel in the base image and checks if a SpriteObj for that color already exists.
- If a SpriteObj for that color exists, it adds the current pixel's coordinates to the sprite's `spritePixels` list and adjusts the sprite's bounding box.
- If no sprite exists for the color, a new `SpriteObj` is created for that color and is added to the spriteList.

### `GenerateColorFormat()`
Converts a `Color` object to an array of floats for JSON serialization. The float values are the red, green, and blue components of the color.

### `GenerateID()`
Generates a unique ID for each sprite by incrementing the `givenId` counter.

### `GenerateSpriteObj()`
Creates a new `SpriteObj` for a specific pixel color and its coordinates.

### `SaveSprites()`
This method saves each sprite as a PNG file:

- It calculates the size of each sprite based on its bounding box.
- It creates a new `Texture2D`, fills it with the corresponding color data, and saves the texture as a PNG in the specified directory.
- It also creates a `SpriteObjJSON` for each sprite and adds it to the `spriteListJSON`.

### `CreateJSON()`
Generates a JSON file containing the sprite metadata:

- It serializes the `CombinedJSON` object, which includes the image dimensions and the list of sprite data, and saves it as `map_position.json`.

### `ChangeImageTypes()`
This method ensures that the generated sprite textures are set to the correct type (Sprite) and adjusts the sprite import settings in Unity:

- It changes the texture import settings so the textures are recognized as sprites and sets the import mode to Single.

