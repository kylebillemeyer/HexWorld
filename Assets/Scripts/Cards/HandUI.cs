using UnityEngine;
using HexWorld.Cards;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using HexWord.Util;
using UnityEngine.Rendering;
using HexWord.Battle;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace HexWord.Cards
{
    public class HandUI : MonoBehaviour
    {
        private static readonly float drawLength = 1f;
        private static readonly float drawDelay = .2f;
        private static readonly float shiftLength = 1f;
        private static readonly Vector3 overlapOffset = new Vector3(50f, 0 , 0);
        private static readonly float cardZoomFactor = 1.5f;
        private static readonly float extraZoomOffset = 50f;

        [SerializeField]
        private AnimationCurve drawCurve;
        public AnimationCurve DrawCurve
        {
            get { return drawCurve; }
            set { drawCurve = value; }
        }

        [SerializeField]
        private AnimationCurve shiftCurve;
        public AnimationCurve ShiftCurve
        {
            get { return shiftCurve; }
            set { shiftCurve = value; }
        }

        public bool Busy { get; set; }

        private Vector3 drawAnchor;
        private Vector3 cardStagingAnchor;
        private Vector3 discardAchor;
        private List<CardMeta> cards;
        private bool resetCardPositionsNextFrame = false;
        private StagedMeta stagedMeta;

        public class CardTransform
        {
            public Vector3 StartPos { get; set; }
            public Vector3 EndPos { get; set; }
            public Vector3 StartScale { get; set; }
            public Vector3 EndScale { get; set; }
            public float ElapsedTime { get; set; }
            public float AnimLength { get; set; }
            public bool CanCancel { get; set; }
            public CurveType Curve { get; set; }
        }

        public enum CurveType
        {
            Draw,
            Shift
        }

        public class CardMeta
        {
            public Card Card { get; set; }
            public GameObject Instance { get; set; }
            public CardTransform Animation { get; set; }
        }

        public class StagedMeta
        {
            public int OrigIndex { get; set; }
            public CardMeta CardMeta { get; set; }
        }

        // Use this for initialization
        void Start()
        {
            cards = new List<CardMeta>();
            
            drawAnchor = GetAnchor("DeckUI");
            discardAchor = GetAnchor("DiscardUI");
            cardStagingAnchor = GetAnchor("CardStagingUI");
        }

        private Vector3 GetAnchor(string objName)
        {
            var obj = GameObject.Find(objName);
            var trans = obj.GetComponent<RectTransform>();
            return obj.transform.position + (Vector3)trans.rect.center;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Busy && Input.GetMouseButtonDown(1))
            {
                if (stagedMeta != null)
                {
                    cards.Insert(stagedMeta.OrigIndex, stagedMeta.CardMeta);
                    ResetCardPositions(null);
                    resetCardPositionsNextFrame = false;
                    stagedMeta = null;
                }
            }


            var allCards = cards.Select(x => x).ToList();

            if (stagedMeta != null)
            {
                allCards.Add(stagedMeta.CardMeta);
            }

            var cardsThatCantBeCancelled = allCards
                .Where(c => c.Animation != null)
                .ToList();

            foreach (var meta in cardsThatCantBeCancelled)
            {
                var anim = meta.Animation;
                anim.ElapsedTime = Math.Min(anim.AnimLength, anim.ElapsedTime + Time.deltaTime);
                if (anim.ElapsedTime >= 0)
                {
                    var curve = anim.Curve == CurveType.Draw ? drawCurve : shiftCurve;

                    var timeOnCurve = anim.ElapsedTime / anim.AnimLength;

                    var moveDistance = anim.EndPos - anim.StartPos;
                    var newPos = anim.StartPos + curve.Evaluate(timeOnCurve) * moveDistance;
                    meta.Instance.transform.position = newPos;

                    var scaleChange = anim.EndScale - anim.StartScale;
                    var newScale = anim.StartScale + curve.Evaluate(timeOnCurve) * scaleChange;
                    meta.Instance.transform.localScale = newScale;
                }

                if (anim.ElapsedTime >= anim.AnimLength)
                {
                    meta.Animation = null;
                }
            }

            cardsThatCantBeCancelled = cardsThatCantBeCancelled.Where(x => x.Animation != null && !x.Animation.CanCancel).ToList();
            Busy = cardsThatCantBeCancelled.Count > 0;

            if (!Busy && resetCardPositionsNextFrame)
            {
                ResetCardPositions(null);
                resetCardPositionsNextFrame = false;
            }
        }

        public void Draw(Card card)
        {
            Draw(new List<Card>() { card });
        }

        public void Draw(List<Card> cs)
        {
            Busy = true;

            var n = cards.Count + cs.Count;

            if (cards.Count > 0)
            {
                var x = 0;
                foreach (var meta in cards)
                {
                    meta.Animation = CreateShiftTransform(meta.Instance, recalculateOffset(n, x));
                    x++;
                }
            }


            var i = cards.Count;
            var j = 0;
            foreach (var c in cs)
            {
                DrawHelper(c, recalculateOffset(n, i), drawDelay * j);
                i++;
                j++;
            }
        }

        private Vector3 recalculateOffset(int n, int i)
        {
            return overlapOffset * (-(n - 1) + 2 * i);
        }

        private void DrawHelper(Card card, Vector3 offset, float delay)
        {
            var cardObj = CreateCardUIElement(card);

            cardObj.transform.position = drawAnchor;
            cardObj.transform.localScale = new Vector3(.5f, .5f, 1);

            var anim = new CardTransform
            {
                StartPos = drawAnchor,
                EndPos = transform.position + (Vector3)GetComponent<RectTransform>().rect.center + offset,
                StartScale = new Vector3(.5f, .5f, 1),
                EndScale = Vector3.one,
                ElapsedTime = -delay,
                AnimLength = drawLength,
                CanCancel = false,
                Curve = CurveType.Draw
            };

            cards.Add(new CardMeta() { Card = card, Instance = cardObj, Animation = anim });
        }

        private CardTransform CreateShiftTransform(GameObject card, Vector3 offset)
        {
            return new CardTransform()
            {
                StartPos = card.transform.position,
                EndPos = transform.position + (Vector3)GetComponent<RectTransform>().rect.center + offset,
                StartScale = card.transform.localScale,
                EndScale = card.transform.localScale,
                ElapsedTime = 0,
                AnimLength = shiftLength,
                CanCancel = false,
                Curve = CurveType.Draw
            };
        }

        public void Discard(Card card)
        {
            //cards.Remove(card);

            // Do animation
        }

        private GameObject CreateCardUIElement(Card card)
        {
            var fab = CardCache.Prefabs[card.RefId];
            
            var inst = Instantiate(fab);
            inst.transform.parent = gameObject.transform;

            var btn = inst.FindObject("CardBtn");
            
            var trigger = btn.GetComponent<EventTrigger>();

            var pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener(data => OnCardEntered((PointerEventData)data, inst));

            var pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener(data => OnCardExited((PointerEventData)data, inst));

            var click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerClick;
            click.callback.AddListener(data => OnCardClicked((PointerEventData)data, inst));

            trigger.triggers.Add(pointerEnter);
            trigger.triggers.Add(pointerExit);
            trigger.triggers.Add(click);

            var titleText = btn.FindObject("TitleText").GetComponent<Text>();
            titleText.text = card.CardName;

            var descriptionText = btn.FindObject("DescriptionText").GetComponent<Text>();
            descriptionText.text = card.Description;

            var cardTypeText = btn.FindObject("TypeText").GetComponent<Text>();
            cardTypeText.text = card.Type.ToString();

            var costText = btn.FindObject("CostText").GetComponent<Text>();
            costText.text = card.Cost.ToString();

            return inst;
        }

        private void OnCardEntered(PointerEventData data, GameObject card)
        {
            if (!Busy)
            {
                if (resetCardPositionsNextFrame)
                {
                    resetCardPositionsNextFrame = false;
                }

                var n = cards.Count;
                var pivot = 0;

                var i = 0;
                foreach (var meta in cards)
                {
                    if (meta.Instance == card)
                    {
                        pivot = i;
                        Vector3 pos = transform.position + (Vector3)GetComponent<RectTransform>().rect.center;
                        meta.Animation = new CardTransform()
                        {
                            StartPos = meta.Instance.transform.position,
                            EndPos = pos + recalculateOffset(n, i),
                            StartScale = meta.Instance.transform.localScale,
                            EndScale = meta.Instance.transform.localScale * cardZoomFactor,
                            ElapsedTime = 0,
                            AnimLength = .1f,
                            CanCancel = true,
                            Curve = CurveType.Shift
                        };
                        meta.Instance.transform.SetAsLastSibling(); // Make render last so it appears on top, fuck unity.
                    }
                    i++;
                }

                ResetCardPositions(pivot);
            }
        }

        private void OnCardExited(PointerEventData data, GameObject card)
        {
            if (!Busy)
            {
                resetCardPositionsNextFrame = true;
            }
        }

        private void OnCardClicked(PointerEventData data, GameObject card)
        {
            if (!Busy && stagedMeta == null && data.button == PointerEventData.InputButton.Left)
            {
                var i = 0;
                foreach (var meta in cards)
                {
                    if (meta.Instance == card)
                    {
                        stagedMeta = new StagedMeta() { OrigIndex = i, CardMeta = meta };

                        meta.Animation = new CardTransform()
                        {
                            StartPos = meta.Instance.transform.position,
                            EndPos = cardStagingAnchor,
                            StartScale = meta.Instance.transform.localScale,
                            EndScale = Vector3.one,
                            ElapsedTime = 0,
                            AnimLength = .3f,
                            CanCancel = false,
                            Curve = CurveType.Shift
                        };
                    }
                    i++;
                }

                cards.RemoveAt(stagedMeta.OrigIndex);
                resetCardPositionsNextFrame = true;
            }
        }

        private void ResetCardPositions(int? pivot)
        {
            var n = cards.Count;
            var i = 0;
            foreach (var meta in cards)
            {
                if ((pivot == null || i != pivot) && (meta.Animation == null || meta.Animation.CanCancel))
                {
                    Vector3 pos = transform.position + (Vector3)GetComponent<RectTransform>().rect.center;
                    meta.Animation = new CardTransform()
                    {
                        StartPos = meta.Instance.transform.position,
                        EndPos = pos + recalculateOffset(n, i) + extraShift(pivot, i),
                        StartScale = meta.Instance.transform.localScale,
                        EndScale = Vector3.one,
                        ElapsedTime = 0,
                        AnimLength = .1f,
                        CanCancel = true,
                        Curve = CurveType.Shift
                    };
                    meta.Instance.transform.SetSiblingIndex(i);
                }
                i++;
            }
        }

        private Vector3 extraShift(int? pivot, int i)
        {
            if (!pivot.HasValue)
                return Vector3.zero;

            return i < pivot.Value ? new Vector3(-extraZoomOffset, 0, 0) : new Vector3(extraZoomOffset, 0 , 0);
        }
    }
}
