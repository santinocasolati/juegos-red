using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPagesService : BaseService
{
    [System.Serializable]
    private struct UIPage
    {
        public string name;
        public GameObject page;
    }

    [SerializeField] private List<UIPage> pages;

    private void Awake()
    {
        ChangePage(pages[0].name);
    }

    public void ChangePage(string pageName)
    {
        foreach (UIPage p in pages)
        {
            p.page.SetActive(p.name == pageName);
        }
    }
}
