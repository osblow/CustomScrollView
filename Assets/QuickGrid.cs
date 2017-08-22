using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Oslobw.UI.Foundation
{
    /// <summary>
    /// 暂不支持动态grid
    /// </summary>
    public class QuickGrid
    {
        public QuickGrid(RectTransform scrollRect, GridLayoutGroup grid)
        {
            m_scrollRect = scrollRect;
            m_grid = grid;
        }


        RectTransform m_scrollRect;
        GridLayoutGroup m_grid;

        
        public void OnScroll(OrientaionType orientation)
        {
            if (!m_grid)
            {
                return;
            }

            int activeRangeMin;
            int activeRangeMax;

            if (orientation == OrientaionType.Vertical)
            {
                activeRangeMin = Mathf.FloorToInt((m_grid.transform.position.y - m_scrollRect.position.y - m_scrollRect.sizeDelta.y / 2) / m_grid.cellSize.y);
                activeRangeMax = Mathf.FloorToInt((m_grid.transform.position.y - m_scrollRect.position.y + m_scrollRect.sizeDelta.y / 2) / m_grid.cellSize.y);
            }
            else
            {
                activeRangeMin = Mathf.FloorToInt((m_grid.transform.position.x - m_scrollRect.position.x - m_scrollRect.sizeDelta.x / 2) / m_grid.cellSize.x);
                activeRangeMax = Mathf.FloorToInt((m_grid.transform.position.x - m_scrollRect.position.x + m_scrollRect.sizeDelta.x / 2) / m_grid.cellSize.x);
            }
            
            for (int i = 0; i < m_grid.transform.childCount; i++)
            {
                SetActive(m_grid.transform.GetChild(i).gameObject, i >= activeRangeMin && i <= activeRangeMax);
            }
        }

        void SetActive(GameObject g, bool active)
        {
            if(g.activeInHierarchy != active)
            {
                g.SetActive(active);
            }
        }
    }
}
