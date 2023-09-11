#!/bin/bash
wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
apt update
apt install -y dotnet-sdk-6.0
mkdir -v -p ~/.local/share/godot/export_templates
ls /root/.local/share/godot/templates/
mv /root/.local/share/godot/templates/${GODOT_VERSION}.stable.mono ~/.local/share/godot/export_templates/${GODOT_VERSION}.stable.mono

apt install -y blender
sh $(pwd)/.build/setup_blender_editor_path.sh