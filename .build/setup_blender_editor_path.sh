echo $(which blender)
BLENDER_PATH="\\/usr\\/bin"

godot --headless -e --quit-after 1

echo "EDITOR SETTINGS BEFORE ---------------------------------"
cat ~/.config/godot/editor_settings-4.tres
echo "-------------------------------------------------------"

cat ~/.config/godot/editor_settings-4.tres \
    | sed "/^filesystem\\/import\\/blender\\/blender3_path =/s/=.*/= \"$BLENDER_PATH\"/" \
    > ~/.config/godot/editor_settings-4.tres.2

cp ~/.config/godot/editor_settings-4.tres.2 ~/.config/godot/editor_settings-4.tres


echo "EDITOR SETTINGS AFTER ---------------------------------"
cat ~/.config/godot/editor_settings-4.tres
echo "-------------------------------------------------------"
rm ~/.config/godot/editor_settings-4.tres.2