
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot inst;

    //AppDomain��ILRuntime����ڣ��������һ���������б��棬������Ϸȫ�־�һ��������Ϊ��ʾ�����㣬ÿ���������涼��������һ��
    //�������ʽ��Ŀ����ȫ��ֻ����һ��AppDomain
    AppDomain appdomain;
    bool hotFinish;
    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;
    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        hotFinish = false;
        StartCoroutine(LoadHotFixAssembly());

    }
    
  

    IEnumerator LoadHotFixAssembly()
    {
        //����ʵ����ILRuntime��AppDomain��AppDomain��һ��Ӧ�ó�����ÿ��AppDomain����һ��������ɳ��
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        //������Ŀ��Ӧ�������д������ط�����dll�����ߴ����AssetBundle�ж�ȡ��ƽʱ�����Լ�Ϊ����ʾ����ֱ�Ӵ�StreammingAssets�ж�ȡ��
        //��ʽ������ʱ����Ҫ������д������ط���ȡdll

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //���DLL�ļ���ֱ�ӱ���HotFix_Project.sln���ɵģ��Ѿ�����Ŀ�����ú����Ŀ¼ΪStreamingAssets����VS��ֱ�ӱ��뼴�����ɵ���ӦĿ¼�������ֶ�����
        //����Ŀ¼��Assets\Samples\ILRuntime\1.6\Demo\HotFix_Project~
        //���¼���д��ֻΪ��ʾ����û�д����ڱ༭���л���Androidƽ̨�Ķ�ȡ����Ҫ�����޸�
#if UNITY_ANDROID
        WWW www = new WWW(Application.streamingAssetsPath + "/HotProject.dll");
#else
        WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/HotProject.dll");
#endif
        while (!www.isDone)
            yield return null;
        if (!string.IsNullOrEmpty(www.error))
            UnityEngine.Debug.LogError(www.error);
        byte[] dll = www.bytes;
        www.Dispose();

        //PDB�ļ��ǵ������ݿ⣬����Ҫ����־����ʾ������кţ�������ṩPDB�ļ����������ڻ��������ڴ棬��ʽ����ʱ�뽫PDBȥ��������LoadAssembly��ʱ��pdb��null����
#if UNITY_ANDROID
        www = new WWW(Application.streamingAssetsPath + "/HotProject.pdb");
#else
        www = new WWW("file:///" + Application.streamingAssetsPath + "/HotProject.pdb");
#endif
        while (!www.isDone)
            yield return null;
        if (!string.IsNullOrEmpty(www.error))
            UnityEngine.Debug.LogError(www.error);
        byte[] pdb = www.bytes;
        fs = new MemoryStream(dll);
        p = new MemoryStream(pdb);
        try
        {
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("�����ȸ�DLLʧ�ܣ���ȷ���Ѿ�ͨ��VS��Assets/Samples/ILRuntime/1.6/Demo/HotFix_Project/HotFix_Project.sln������ȸ�DLL");
        }

        InitializeILRuntime();
        OnHotFixLoaded();
    }

    void InitializeILRuntime()
    {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
        //����Unity��Profiler�ӿ�ֻ���������߳�ʹ�ã�Ϊ�˱�����쳣����Ҫ����ILRuntime���̵߳��߳�ID������ȷ���������к�ʱ�����Profiler
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        //������һЩILRuntime��ע�ᣬHelloWorldʾ����ʱû����Ҫע���
    }

    void OnHotFixLoaded()
    {
        GameRoot.inst = gameObject.GetComponent<GameRoot>();
        
        //HelloWorld����һ�η�������
        appdomain.Invoke("HotProject.Main", "Init", null, null);
        hotFinish = true;
    }

    private void OnDestroy()
    {
        if (fs != null)
            fs.Close();
        if (p != null)
            p.Close();
        fs = null;
        p = null;
    }

    void Update()
    {
        if (!hotFinish)
            return;
        appdomain.Invoke("HotProject.Main", "Update", null, null);
    }
    public void Print()
    {
        Debug.Log("gameRoot hello!");
    }
}
