applications:
- name: <manifest.App.Name>
  command: <manifest.App.Command>
  buildpacks:
  - <manifest.App.BuildPacks>
  stack: <manifest.App.Stack>
  memory: <manifest.App.Memory>
  <if(manifest.App.Environment)>
  env:
  <manifest.App.Environment:{envname |   <envname>: <manifest.App.Environment.(envname)>}; separator="\n">
  <endif>
  <if(manifest.App.ServiceNames)>
  services:
  <manifest.App.ServiceNames:{svcname|- <svcname>}; separator="\n">
  <endif>
