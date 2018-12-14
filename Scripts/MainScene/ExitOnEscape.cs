using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

/* Summary
 * If escape button is clicked on android(back button), save data and close application
 */
public class ExitOnEscape : MonoBehaviour {
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            await Task.Run(CloverDirector.GetInstance().SetData);
            Application.Quit();
        }
    }
}
