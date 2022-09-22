using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class AsyncTest : MonoBehaviour
{
    public GameObject[] objs;
    
    async void RotateFunc()
    {
        await RotateOneByOne();
        await Task.Delay(1000);//停止1000ms
        RotateTogetherAsnyc();
    }

    public async void RotateTogetherAsnyc()
    {
        Debug.Log("Together start");
        Task[] tasks = new Task[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            //协程和异步一样效果
            //StartCoroutine(Rotate(objs[i], i + 1));
            tasks[i] = RotateAsync(objs[i], i + 1);
        }
        await Task.WhenAll(tasks);//等待所有任务执行完
        Debug.Log("Rotate together is finished");
    }
    
    public void RotateTogetherCor()
    {
        Debug.Log("Together start");
        for (int i = 0; i < objs.Length; i++)
        {
            //协程和异步一样效果
            StartCoroutine(RotateCor(objs[i], i + 1));
        }
        Debug.Log("Rotate together is finished");
    }


    //协程 关键词IEnumerator，yield return null
    //在主线程运行，所以游戏不运行时，只执行一帧
    IEnumerator RotateCor(GameObject obj, float duration)
    {
        float timer = Time.time + duration;
        while (Time.time < timer)
        {
            obj.transform.Rotate(new Vector3(1, 1) * Time.deltaTime * 150);
            yield return null;
        }
    }

    //异步，关键词async, await Task.Yield()
    //在子线程运行，所以游戏不运行时，时间也是在走的
    private async Task RotateAsync(GameObject obj, float duration)
    {
        float timer = Time.time + duration;
        while (Time.time < timer)
        {
            obj.transform.Rotate(new Vector3(1, 1) * Time.deltaTime * 150);
            await Task.Yield();
        }
    }

    public async Task RotateOneByOne()//async异步函数
    {
        Debug.Log("One by one start");
        for (int i = 0; i < objs.Length; i++)
        {
            await RotateAsync(objs[i], i + 1);//等待事情做完
        }
        Debug.Log("One by one finished");
    }

    public void PrintRandomNumber()
    {
        int rand = GetRandomNumber().GetAwaiter().GetResult();//不用await, .Result是另一种获得异步函数结果的方法
        print(rand);
    }

    async Task<int> GetRandomNumber() //带返回值的异步函数
    {
        int rand = Random.Range(300, 500);
        await Task.Delay(rand);
        return rand;
    }
}

[CustomEditor(typeof(AsyncTest))] 
public class AsyncTestWindow : Editor
{
    private AsyncTest m_Target; //在Inspector上显示的实例目标
    /// <summary>
    /// 当对象活跃时（在Inspector中显示时），unity自动调用此函数
    /// </summary>
    private void OnEnable()
    {
        m_Target = target as AsyncTest; //绑定target，target官方解释： The object being inspected
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Async rotate together"))
        {
            m_Target.RotateTogetherAsnyc();
        }
        if (GUILayout.Button("Coroutine rotate together"))
        {
            m_Target.RotateTogetherCor();
        }
        if (GUILayout.Button("Async rotate one by one"))
        {
            m_Target.RotateOneByOne();
        }
        // if (GUILayout.Button("Print random number"))
        // {
        //     m_Target.PrintRandomNumber();
        // }
    }
}