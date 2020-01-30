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
  replicas: <config.Spec.Replicas>
  <if (config.Spec.Selector.MatchLabels)>
  selector:
    matchLabels:
      <config.Spec.Selector.MatchLabels: {label|<label>: <config.Spec.Selector.MatchLabels.(label)>}; separator="\n">
  <endif>
  template:
  <if (config.Spec.Template.MetaData.Labels)>
    metadata:
      labels:
        <config.Spec.Template.MetaData.Labels: {label|<label>: <config.Spec.Template.MetaData.Labels.(label)>}; separator="\n">
  <endif>
    spec:
      containers:
      <config.Spec.Template.Spec.Containers:
      {container|
- name: <container.Name>
  image: <container.Image>
  ports:
  <container.Ports: {port| - containerPort: <port.ContainerPort>}>}; separator="\n">
