using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemContainer<T, U> where T : ContainerItem<U> {

    public List<T> Items { get; }

    private T template;

    public ItemContainer(T itemTemplate) {
        template = itemTemplate;
        Items = new List<T>();

        itemTemplate.gameObject.SetActive(false);
    }

    public T Get(U data) {
        for (int i = 0; i < Items.Count; i++) {
            if (EqualityComparer<U>.Default.Equals(Items[i].Data, data)) {
                return Items[i];
            }
        }
        return null;
    }

    public void UpdateContainer(IEnumerable<U> dataCollection) {
        if (dataCollection == null) { return; }

        List<T> oldItemList = new List<T>(Items);
        Items.Clear();
        
        foreach (U data in dataCollection) {
            T item = oldItemList.FirstOrDefault(x => x.Data != null && x.Data.Equals(data));

            if (item != null) {
                item.Setup(data);
                oldItemList.Remove(item);
                Items.Add(item);
            } else {
                T newItem = GameObject.Instantiate(template, template.transform.parent);
                newItem.Setup(data);
                Items.Add(newItem);
            }
        }

        DestroyItems(oldItemList);
    }

    private void DestroyItems(IEnumerable<T> itemCollection) {
        foreach (T item in itemCollection) {
            item.Dispose();
            GameObject.Destroy(item.gameObject);
        }
    }

}