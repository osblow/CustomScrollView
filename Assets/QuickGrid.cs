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
                activeRangeMin = Mathf.FloorToInt((m_grid.transform.position.y - m_scrollRect.position.y - m_scrollRect.sizeDelta.y / 2) / (m_grid.cellSize.y + m_grid.spacing.y));
                activeRangeMax = Mathf.FloorToInt((m_grid.transform.position.y - m_scrollRect.position.y + m_scrollRect.sizeDelta.y / 2) / (m_grid.cellSize.y + m_grid.spacing.y));
            }
            else
            {
                activeRangeMin = Mathf.FloorToInt((m_grid.transform.position.x - m_scrollRect.position.x - m_scrollRect.sizeDelta.x / 2) / (m_grid.cellSize.x + m_grid.spacing.x));
                activeRangeMax = Mathf.FloorToInt((m_grid.transform.position.x - m_scrollRect.position.x + m_scrollRect.sizeDelta.x / 2) / (m_grid.cellSize.x + m_grid.spacing.x));
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


        public bool CheckBounds(OrientaionType orientation, ref Vector3 curTargetPos)
        {
            if (!m_grid)
            {
                return true;
            }

            Vector3 targetPosWorld = m_grid.transform.parent.TransformPoint(curTargetPos);

            if (orientation == OrientaionType.Vertical)
            {
                if(targetPosWorld.y - m_scrollRect.position.y - m_scrollRect.sizeDelta.y / 2 < 0)
                {
                    curTargetPos = GetAdjustGridPos(float.MaxValue, m_scrollRect.position.y + m_scrollRect.sizeDelta.y / 2);
                    return false;
                }

                float gridHeight = m_grid.transform.childCount * (m_grid.cellSize.y + m_grid.spacing.y);
                if(targetPosWorld.y - m_scrollRect.position.y + m_scrollRect.sizeDelta.y / 2 > gridHeight)
                {
                    curTargetPos = GetAdjustGridPos(float.MaxValue, m_scrollRect.position.y - m_scrollRect.sizeDelta.y / 2 + gridHeight);
                    return false;
                }
            }
            else
            {
                if(targetPosWorld.x - m_scrollRect.position.x - m_scrollRect.sizeDelta.x / 2 < 0)
                {
                    curTargetPos = GetAdjustGridPos(m_scrollRect.position.x + m_scrollRect.sizeDelta.x / 2, float.MaxValue);
                    return false;
                }

                float gridWidth = m_grid.transform.childCount * (m_grid.cellSize.x + m_grid.spacing.x);
                if(targetPosWorld.x - m_scrollRect.position.x + m_scrollRect.sizeDelta.x / 2 > gridWidth)
                {
                    curTargetPos = GetAdjustGridPos(m_scrollRect.position.x - m_scrollRect.sizeDelta.x / 2 + gridWidth, float.MaxValue);
                    return false;
                }
            }

            return true;
        }

        private Vector3 GetAdjustGridPos(float targetX, float targetY)
        {
            Vector3 curPos = m_grid.transform.position;
            if(targetX < float.MaxValue)
            {
                curPos.x = targetX;
                return m_grid.transform.parent.InverseTransformPoint(curPos);
            }
            if (targetY < float.MaxValue)
            {
                curPos.y = targetY;
                return m_grid.transform.parent.InverseTransformPoint(curPos);
            }

            return Vector3.zero;
        }
    }
}
