using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ListOrderByName {
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
