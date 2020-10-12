using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu(fileName = "ShaderList", menuName = "ScriptableObjects/ShaderList", order = 1)]
public class ShaderListScritableObj : ScriptableObject {
    public List<Shader> shaders;
    public void AddShaders(List<Shader> shaders) {

        this.shaders = OrderByShaderName(shaders);
    }

    private List<Shader> OrderByShaderName(List<Shader> shaders) {
        if(shaders == null || shaders.Count <= 1) {
            return null;
        }
        shaders.Sort(
                (x, y) => string.Compare(x.name, y.name)
            );
        var orderedShaders = shaders.OrderBy(
                x => x.name
            ).ToList();
        return orderedShaders;
    }
}
