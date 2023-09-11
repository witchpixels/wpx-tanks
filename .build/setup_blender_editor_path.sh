BLENDER_PATH=$(which blender) || BLENDER_PATH="\\/usr\\/bin"

cat ~/.config/godot/editor_settings-4.tres \
    | sed "/^filesystem\\/import\\/blender\\/blender3_path =/s/=.*/= \"$BLENDER_PATH\"/" \
    > ~/.config/godot/editor_settings-4.tres.2

cp ~/.config/godot/editor_settings-4.tres.2 ~/.config/godot/editor_settings-4.tres
cat ~/.config/godot/editor_settings-4.tres
rm ~/.config/godot/editor_settings-4.tres.2