using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameMap GMap { get; private set; }

    void Start()
    {                   
    }

    public void Clear()
    {
        GMap = null;
    }
    
    public IEnumerator CreateMap(string mapFileName)
    {
        //GameObject player = Resources.Load<GameObject>(PLAYER_PREFAB_PATH);

        Debug.Log($"Load Map with name = {mapFileName}");

        yield return new WaitForSeconds(0.1f);
        yield return null;

        GameObject gameItem = Resources.Load<GameObject>(mapFileName);


        yield return new WaitForSeconds(0.1f);
        yield return null;

        GameObject instance = Instantiate(gameItem);
        GMap = instance?.GetComponent<GameMap>();


        yield return new WaitForSeconds(0.1f);
        yield return null;

        //Instantiate(player,GMap.GetComponent<GameMap>().Py_Spawn_local,Quaternion.identity);
        //Instantiate(player, GMap.GetComponent<GameMap>().Wposition, Quaternion.identity);

    }

    // Update is called once per frame
    public async Task<GameMap> CreateMapAsync(string mapFileName)
    {
        var ret = Resources.LoadAsync(mapFileName);
        //await ret;
        return GMap;
    }
}
