﻿#if TinaX_CA_LuaRuntime_Enable

using System;
using UnityEngine;
#if UNITY_EDITOR && ODIN_INSPECTOR
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

#if UNITY_EDITOR && ODIN_INSPECTOR

namespace TinaX.Lua
{
    public class LuaBehaviourBindEditor : OdinEditorWindow
    {

        [TabGroup("对象注入绑定")]
        [TableList]
        [Title("对象注入绑定")]
        [OnValueChanged("OnInjectionChanged",true)]
        public Injection[] b_injection;

        [TabGroup("文本注入绑定")]
        [TableList]
        [Title("文本注入绑定")]
        public Injection_String[] b_injection_str;

        private LuaBehaviour mTarget;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.titleContent = new GUIContent("注入绑定编辑器");
            mTarget = Selection.activeGameObject.GetComponent<LuaBehaviour>();

            if (mTarget == null)
            {
                EditorUtility.DisplayDialog("Error", "编辑器初始化失败", "好");
                return;
            }
            this.UseScrollView = true;
            b_injection = mTarget.Injections;
            b_injection_str = mTarget.Injections_str;

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (mTarget != null)
            {
                mTarget.Injections = b_injection;
                mTarget.Injections_str = b_injection_str;

                EditorUtility.SetDirty(mTarget.gameObject);
                EditorUtility.SetDirty(mTarget);

                
            }
            
        }

        private void OnInjectionChanged()
        {
            foreach(var item in b_injection)
            {
                if (item.Object != null && string.IsNullOrEmpty(item.Name))
                {
                    //Debug.Log("新拖上来一个东西：" + item.Object.name + " | " + item.Object.GetType().Name);
                    var typeName = item.Object.GetType().Name;
                    var objectName = item.Object.name;

                    void HandlePrefix(string prefix)
                    {
                        if (!objectName.StartsWith(prefix))
                        {
                            item.Name = prefix + objectName;
                        }
                        else
                        {
                            item.Name = objectName;
                        }
                    }

                    switch (typeName)
                    {
                        default:
                            item.Name = objectName;
                            break;
                        case "GameObject":
                            HandlePrefix("go_");
                            break;
                        case "Text":
                            HandlePrefix("txt_");
                            break;
                        case "XText":
                            HandlePrefix("txt_");
                            break;
                        case "Image":
                            HandlePrefix("img_");
                            break;
                        case "XImage":
                            HandlePrefix("img_");
                            break;
                        case "Button":
                            HandlePrefix("btn_");
                            break;
                        case "XButton":
                            HandlePrefix("btn_");
                            break;
                    }
                }
            }
        }

        
    }
}

#endif


#endif