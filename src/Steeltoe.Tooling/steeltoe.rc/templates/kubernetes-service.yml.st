apiVersion: <config.ApiVersion>
kind: <config.Kind>
<if(config.MetaData)>
metadata:
  name: <config.MetaData.Name>
  <if (config.MetaData.Labels)>
  labels:
    <config.MetaData.Labels: {label|<label>: <config.MetaData.Labels.(label)>}>
  <endif>
<endif>
spec:
  type: <config.Spec.Type>
  <if (config.Spec.Selector)>
  selector:
    <config.Spec.Selector:{selector |<selector>: <config.Spec.Selector.(selector)>}; separator="\n">
  <endif>
  <if (config.Spec.Ports)>
  ports:
  <config.Spec.Ports:
  {port|
- port: <port.Port>
  targetPort: <port.TargetPort>}; separator="\n">
  <endif>
