#!/bin/bash

if [[ -z "$GitVersion_FullSemVer" ]]; then
  GitVersion_FullSemVer="0.1.0-failed-to-set-from-gitversion"
fi

cat ./project.godot | sed "/^config\\/version=/s/=.*/=\"$GitVersion_FullSemVer\"/" > ./project.godot.2
cp ./project.godot.2 ./project.godot
rm ./project.godot.2
