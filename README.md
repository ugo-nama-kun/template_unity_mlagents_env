# template_unity_mlagents_env
This is a template unity setting of the mlagents for deep model training

Unity MLAgents is awesome tool for AI researches. However, documentation for python client is not enough and sometimes I got stucked.
In this repository, I added several tricks of MLAgents, mainly intended for the agent with vision inputs.

The example project here includes

1. Disabling autonomous physics simulation and hence Unity updates physics only when rendering was completed
2. Adjust frame rate by timeScale (except VR condition)
3. Add Util classes, provided by Example of the MLAgents  
4. Use Monitor util class for quick visualization
5. Add the example usage of TextureRenderer as image inputs of the agent.

Main suggestions of C# code are written in [AgentTemplate.cs](https://github.com/ugo-nama-kun/template_unity_mlagents_env/blob/master/template_mlagents_env/Assets/Scripts/AgentTemplate.cs).

## Unity project on github
I followed the instruction by German at stackoverflow.
https://stackoverflow.com/questions/21573405/how-to-prepare-a-unity-project-for-git
