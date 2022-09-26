using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UniTaskTest : MonoBehaviour
{
    public Button loadTestBtn;
    public Text text;
    
    private void Start()
    {
        //loadTestBtn.onClick.AddListener(OnClickLoadText);
        loadTestBtn.onClick.AddListener(OnClickLoadScene);
    }

    public async UniTask<Object> LoadAsnyc<T>(string path) where T : Object
    {
        ResourceRequest rr = Resources.LoadAsync<T>(path);
        return await rr;
    }

    private async void OnClickLoadText()
    {
        // ResourceRequest rr =  Resources.LoadAsync<TextAsset>("test");
        // Object str = await rr;
        // text.text = ((TextAsset)str).text;
        text.text = ((TextAsset)await LoadAsnyc<TextAsset>("test2")).text;
    }

    public Slider slider;
    private async void OnClickLoadScene()
    {
        await SceneManager.LoadSceneAsync("Scenes/NewScene",LoadSceneMode.Additive).ToUniTask(
            Progress.Create<float>(p =>
            {
                slider.value = p;
                text.text = $"读取进度：{p*100:F2}%";
            }));
    }
}
