using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Oslobw.UI.Foundation
{
    public enum OrientaionType
    {
        Horizontal = 0,
        Vertical = 1
    }

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    public class CustomScrollRect : MonoBehaviour
    {
        public OrientaionType Orientation;

        [Space]
        public Transform Content;
        public float MoveSpeed = 800;



        private GridLayoutGroup m_grid;
        private QuickGrid m_quickGrid;

        private Vector3 m_lastMousePos;
        private Vector3 m_curMousePos;

        private Vector3 m_contentMoveTarget = Vector3.zero;
        private bool m_startMove = false;

        private const float c_move_sensitive = 10;


        // Use this for initialization
        void Start()
        {
            if (!Content)
            {
                Debug.LogError("No scroll content attached!!");
                return;
            }
            
            m_contentMoveTarget = Content.position;
            m_grid = Content.GetComponent<GridLayoutGroup>();
            m_quickGrid = new QuickGrid(GetComponent<RectTransform>(), m_grid);

            if(m_grid) StartCoroutine(DisableGridLater());
        }

        // Update is called once per frame
        void Update()
        {
            if (!CheckValid())
            {
                return;
            }


            if (Input.GetMouseButtonDown(0))
            {
                m_lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                m_curMousePos = Input.mousePosition;

                Vector2 mouseSpeed = GetMoveSpeed();

                if (Orientation == OrientaionType.Vertical)
                {
                    // vertical
                    if (Mathf.Abs(mouseSpeed.y) > c_move_sensitive)
                    {
                        m_contentMoveTarget = Content.localPosition + Vector3.up * mouseSpeed.y;
                        m_startMove = true;
                        m_quickGrid.OnScroll(Orientation);
                    }
                }
                else
                {
                    // horizontal
                    if (Mathf.Abs(mouseSpeed.x) > c_move_sensitive)
                    {
                        m_contentMoveTarget = Content.localPosition + Vector3.right * mouseSpeed.x;
                        m_startMove = true;
                        m_quickGrid.OnScroll(Orientation);
                    }
                }

                m_lastMousePos = m_curMousePos;
            }

            Move();
        }

        void Move()
        {
            if (!m_startMove)
            {
                return;
            }

            if(Vector3.Distance(Content.position, m_contentMoveTarget) < 1)
            {
                m_startMove = false;
                return;
            }

            Content.localPosition = Vector3.MoveTowards(Content.localPosition, m_contentMoveTarget, Time.deltaTime * MoveSpeed);
        }


        Vector2 m_mouseSpeed = new Vector2();
        Vector2 GetMoveSpeed()
        {
            m_mouseSpeed.x = m_curMousePos.x - m_lastMousePos.x;
            m_mouseSpeed.y = m_curMousePos.y - m_lastMousePos.y;
            if(m_mouseSpeed.x < 0.5f && m_mouseSpeed.x > -0.5f)
            {
                m_mouseSpeed.x = 0;
            }
            if(m_mouseSpeed.y < 0.5f && m_mouseSpeed.y > -0.5f)
            {
                m_mouseSpeed.y = 0;
            }
            
            return m_mouseSpeed;
        }


        bool CheckValid()
        {
            if (!Content)
            {
                return false;
            }

            return true;
        }


        IEnumerator DisableGridLater()
        {
            yield return new WaitForSeconds(0.3f);
            m_grid.enabled = false;
        }
    }
}
