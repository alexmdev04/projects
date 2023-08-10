//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e7240cfa-6e55-47b6-b74d-b539048b211c"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""18a0da52-af71-42eb-a3ca-e523384784fa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""716e10d4-2c92-4d3d-b348-18d3cc680d56"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""b8abb189-d1dc-43b6-ae86-d6a33ccd67af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlot1"",
                    ""type"": ""Button"",
                    ""id"": ""f932e87c-a73d-4d00-9b84-1c495d850262"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlot2"",
                    ""type"": ""Button"",
                    ""id"": ""5f540b5e-dbed-4077-a73e-0f2973233998"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MeleeWeaponSlot"",
                    ""type"": ""Button"",
                    ""id"": ""37a1fb60-5848-48a7-bb2c-a36fe74cbc78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipmentSlot1"",
                    ""type"": ""Button"",
                    ""id"": ""9aa3eab9-1bfe-4834-8839-22368e6101be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""97e6b767-4d65-408d-a419-ff09bb4b9f21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlotUp"",
                    ""type"": ""Value"",
                    ""id"": ""df930921-d408-473e-b815-16fed8206217"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""WeaponSlotDown"",
                    ""type"": ""Value"",
                    ""id"": ""e31b5e67-fb95-4733-a170-ecdfdcf37adc"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Mouse"",
                    ""id"": ""663f16f7-8238-48cb-acc1-dab1dd1fca51"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""93184b81-99ca-4f85-b73a-98f3fa6bfefc"",
                    ""path"": ""<Mouse>/delta/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f88221f2-2b3b-4977-87dd-a53b9437c9f2"",
                    ""path"": ""<Mouse>/delta/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e39fcc92-d369-47a3-9ad7-33e760454a48"",
                    ""path"": ""<Mouse>/delta/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""217cfaab-5f90-4646-bcad-bd958f1fe018"",
                    ""path"": ""<Mouse>/delta/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""670b89a3-b8a9-42aa-ae94-3be9bae239ab"",
                    ""path"": ""3DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""27821880-b4e3-4043-9c9d-184ca75d39ab"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""65655aac-6310-42ad-886d-563b518723d7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""b4357441-e9fb-419e-a6e7-fdb7250f23df"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""5c58f3c6-66f6-4225-9d58-7eb86b529183"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8fb83cbe-6f6e-4846-83d2-2fea18a8b07f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc3ca914-fa78-4711-be6d-969a21c78de9"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""WeaponSlot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d46bf54-1bcf-4b4b-abf1-b2ffe2e34689"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""WeaponSlot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0659a25e-2562-487d-9498-96c739afa1bb"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""EquipmentSlot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15ff9439-bf0a-4de7-9c69-058d39a3ca7c"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""MeleeWeaponSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5b492d2-123c-4914-b81a-52d621805810"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bba28172-18a4-4914-b015-acbe769e6f77"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""WeaponSlotUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""223474a3-7b8f-4e73-aad6-c341a3406582"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""WeaponSlotDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KBM"",
            ""bindingGroup"": ""KBM"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_WeaponSlot1 = m_Player.FindAction("WeaponSlot1", throwIfNotFound: true);
        m_Player_WeaponSlot2 = m_Player.FindAction("WeaponSlot2", throwIfNotFound: true);
        m_Player_MeleeWeaponSlot = m_Player.FindAction("MeleeWeaponSlot", throwIfNotFound: true);
        m_Player_EquipmentSlot1 = m_Player.FindAction("EquipmentSlot1", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_WeaponSlotUp = m_Player.FindAction("WeaponSlotUp", throwIfNotFound: true);
        m_Player_WeaponSlotDown = m_Player.FindAction("WeaponSlotDown", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_WeaponSlot1;
    private readonly InputAction m_Player_WeaponSlot2;
    private readonly InputAction m_Player_MeleeWeaponSlot;
    private readonly InputAction m_Player_EquipmentSlot1;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_WeaponSlotUp;
    private readonly InputAction m_Player_WeaponSlotDown;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @WeaponSlot1 => m_Wrapper.m_Player_WeaponSlot1;
        public InputAction @WeaponSlot2 => m_Wrapper.m_Player_WeaponSlot2;
        public InputAction @MeleeWeaponSlot => m_Wrapper.m_Player_MeleeWeaponSlot;
        public InputAction @EquipmentSlot1 => m_Wrapper.m_Player_EquipmentSlot1;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @WeaponSlotUp => m_Wrapper.m_Player_WeaponSlotUp;
        public InputAction @WeaponSlotDown => m_Wrapper.m_Player_WeaponSlotDown;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @WeaponSlot1.started += instance.OnWeaponSlot1;
            @WeaponSlot1.performed += instance.OnWeaponSlot1;
            @WeaponSlot1.canceled += instance.OnWeaponSlot1;
            @WeaponSlot2.started += instance.OnWeaponSlot2;
            @WeaponSlot2.performed += instance.OnWeaponSlot2;
            @WeaponSlot2.canceled += instance.OnWeaponSlot2;
            @MeleeWeaponSlot.started += instance.OnMeleeWeaponSlot;
            @MeleeWeaponSlot.performed += instance.OnMeleeWeaponSlot;
            @MeleeWeaponSlot.canceled += instance.OnMeleeWeaponSlot;
            @EquipmentSlot1.started += instance.OnEquipmentSlot1;
            @EquipmentSlot1.performed += instance.OnEquipmentSlot1;
            @EquipmentSlot1.canceled += instance.OnEquipmentSlot1;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @WeaponSlotUp.started += instance.OnWeaponSlotUp;
            @WeaponSlotUp.performed += instance.OnWeaponSlotUp;
            @WeaponSlotUp.canceled += instance.OnWeaponSlotUp;
            @WeaponSlotDown.started += instance.OnWeaponSlotDown;
            @WeaponSlotDown.performed += instance.OnWeaponSlotDown;
            @WeaponSlotDown.canceled += instance.OnWeaponSlotDown;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @WeaponSlot1.started -= instance.OnWeaponSlot1;
            @WeaponSlot1.performed -= instance.OnWeaponSlot1;
            @WeaponSlot1.canceled -= instance.OnWeaponSlot1;
            @WeaponSlot2.started -= instance.OnWeaponSlot2;
            @WeaponSlot2.performed -= instance.OnWeaponSlot2;
            @WeaponSlot2.canceled -= instance.OnWeaponSlot2;
            @MeleeWeaponSlot.started -= instance.OnMeleeWeaponSlot;
            @MeleeWeaponSlot.performed -= instance.OnMeleeWeaponSlot;
            @MeleeWeaponSlot.canceled -= instance.OnMeleeWeaponSlot;
            @EquipmentSlot1.started -= instance.OnEquipmentSlot1;
            @EquipmentSlot1.performed -= instance.OnEquipmentSlot1;
            @EquipmentSlot1.canceled -= instance.OnEquipmentSlot1;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @WeaponSlotUp.started -= instance.OnWeaponSlotUp;
            @WeaponSlotUp.performed -= instance.OnWeaponSlotUp;
            @WeaponSlotUp.canceled -= instance.OnWeaponSlotUp;
            @WeaponSlotDown.started -= instance.OnWeaponSlotDown;
            @WeaponSlotDown.performed -= instance.OnWeaponSlotDown;
            @WeaponSlotDown.canceled -= instance.OnWeaponSlotDown;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KBMSchemeIndex = -1;
    public InputControlScheme KBMScheme
    {
        get
        {
            if (m_KBMSchemeIndex == -1) m_KBMSchemeIndex = asset.FindControlSchemeIndex("KBM");
            return asset.controlSchemes[m_KBMSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnWeaponSlot1(InputAction.CallbackContext context);
        void OnWeaponSlot2(InputAction.CallbackContext context);
        void OnMeleeWeaponSlot(InputAction.CallbackContext context);
        void OnEquipmentSlot1(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnWeaponSlotUp(InputAction.CallbackContext context);
        void OnWeaponSlotDown(InputAction.CallbackContext context);
    }
}