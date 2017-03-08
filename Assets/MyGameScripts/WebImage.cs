using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class WebImage : MonoBehaviour 
{
    /// <summary>
    /// 加载到的纹理
    /// </summary>
    private Texture mTexture; //占用内存，使用后立即释放

    /// <summary>
    /// 创建的地图集
    /// </summary>
    private UIAtlas mUiAtlas; //占用内存，使用后立即释放

    /// <summary>
    /// 图片地址
    /// </summary>
    public string url = string.Empty;

    /// <summary>
    /// 缓存的基础路径
    /// </summary>
    string BasePath
    {
        get
        {
            return Application.persistentDataPath + "/Images/";
           // return "D:/imgs/Images/";
        }
    }

	void Start () 
    {
        if(!string.IsNullOrEmpty(url))
        {
             StartCoroutine(LoadImage(url));
        }
	}

    void OnDestroy()
    {
        GameObject.Destroy(mTexture);
        GameObject.Destroy(mUiAtlas);
    }

    /// <summary>
    /// 填充纹理
    /// </summary>
    void SetTexture(Texture2D img)
    {
        mTexture = img;

        //GUI texture
        if(guiTexture != null)
        {
           guiTexture.texture = img;
           return;
        }

        //NGUI texture
        UITexture texture = GetComponent<UITexture>();
        if(texture != null)
        {
            texture.mainTexture = img;
            return;
        }
        /*
        //NGUI sprite
        UISprite sprite = GetComponent<UISprite>();
        if(sprite != null)
        {
            img.name = "main";
            List<Texture> texs = new List<Texture>();
            texs.Add(img);
            mUiAtlas = CreatAtlas.CreatAtlasFromTex(texs);

            sprite.atlas = mUiAtlas;
            sprite.spriteName = "main";
            return;
        }
         * */
    }

    /// <summary>
    /// 获取缓存路径
    /// </summary>
    string Path(string url)
    {
        return  BasePath + MD5(url) + ".img";
    }

    /// <summary>
    /// 检测缓存是否存在
    /// </summary>
    bool Exits(string url)
    {
#if !UNITY_WEBPLAYER
        return File.Exists(Path(url));
#else
        return false;
#endif
    }

    /// <summary>
    /// 下载图片
    /// </summary>
    IEnumerator LoadImage(string url)
    {
        bool exist = Exits(url);

        if (exist)
        {
            url = "file:///"+Path(url);
            print("url = "+url);
        }
        else
        {
            Debug.Log("load image from www:" + url);
        }

        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error) && !CheckIsDefultImage(www.texture))
        {
            SetTexture(www.texture);
            if (!exist)
            {
                Save(www);
            }
        }
        else
        {
            Debug.LogError("download image [url=" + www.url + "] error!" + www.error);
        }
    }

    /// <summary>
    /// 保持图片
    /// </summary>
    void Save(WWW www)
    {
#if !UNITY_WEBPLAYER
        try
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            File.WriteAllBytes(Path(www.url),www.bytes);
        }
        catch(Exception e)
        {
            Debug.LogError("save image error ! "+e.Message);
        }
#endif
    }

    /// <summary>
    /// MD5 string 
    /// </summary>
    string MD5(string data)
    {
        return MD5(Encoding.Default.GetBytes(data.Trim()));
    }

    /// <summary>
    /// MD5 bytes
    /// </summary>
    string MD5(byte[] data)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] bytes = md5.ComputeHash(data);
        md5.Clear();

        string str = "";
        for (int i = 0; i < bytes.Length - 1; i++)
        {
            str += bytes[i].ToString("x").PadLeft(2, '0');
        }
        return str;
    }

    /// <summary>
    /// 检测从web得到的图片是否是系统默认的？号图片
    /// </summary>
    bool CheckIsDefultImage(Texture2D texture)
    {
        //默认图片大小是8*8的
        if(texture.width != 8 || texture.height != 8)
        {
            return false;
        }

        string md5 = "268D45E0A005CF993B3DD90495A949"; //默认？图片MD5 值

        if(MD5(texture.EncodeToPNG()).ToUpper() != md5)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 替换图片
    /// </summary>
    public void Replace(string newUrl)
    {
        url = newUrl;
        Start();
    }

    /// <summary>
    /// 设置web图片
    /// </summary>
    public static WebImage Set(GameObject obj, string url)
    { 
        if(obj == null || string.IsNullOrEmpty(url))
        {
            return null;
        }
        WebImage img = obj.AddComponent<WebImage>();
        img.url = url;
        return img;
    }
}