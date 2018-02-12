using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : BaseController {
	public Canvas CanvasObject;
	public GameObject GridPrefab;
	public GameObject BlockPrefab;
	public GameObject BoardBackgroundPrefab;
	public GameObject CharacterBackgroundPrefab;
	public GameObject CharacterPrefab;
	public GameObject OpponentPrefab;

	public Slider MPBar;
	public Slider HPBar; 
	public Transform CharacterLayoutContainer;

	// Use this for initialization
	void Start () {

		// Add systems here
		AnimationSystem ans = new AnimationSystem();
		AddSystem(ans);
		TouchSystem ts = new TouchSystem();
		AddSystem(ts);

		CharacterSystem chs = new CharacterSystem();
		AddSystem(chs);
		OpponentSystem opps = new OpponentSystem();
		AddSystem(opps);

		MovingGridSystem mgs = new MovingGridSystem();
		AddSystem(mgs);
		ColorSystem cs = new ColorSystem();
		AddSystem(cs);
		CursorSystem cus = new CursorSystem();
		AddSystem(cus);
		SwapSystem ss = new SwapSystem();
		AddSystem(ss);
		FallSystem fs = new FallSystem();
		AddSystem(fs);
		MatchingSystem ms = new MatchingSystem();
		AddSystem(ms);
		BlockSystem bs = new BlockSystem();
		AddSystem(bs);

		MagicSystem magicSystem = new MagicSystem();
		AddSystem(magicSystem);
		HealthSystem healthSystem = new HealthSystem();
		AddSystem(healthSystem);
		WarningSystem warningSystem = new WarningSystem();
		AddSystem(warningSystem);
		AttackSystem attackSystem = new AttackSystem();
		AddSystem(attackSystem);

		DestroySystem ds = new DestroySystem();
		AddSystem(ds);

		Enable();

		StartGame();

//		FixUI();

		ExtraSetup();
	}

	private void ExtraSetup() {
		DOTween.useSmoothDeltaTime = true;
	}

	private void FixUI() {
		for (int i = 0; i < CanvasObject.transform.childCount; i++) {
			Transform t = CanvasObject.transform.GetChild(i);
			t.localScale = Vector3.one * Utils.CameraScale;
		}
	}

	public void OnBlockButtonPressed() {
		MovingGridSystem mgs = GetSystem<MovingGridSystem>();
		mgs.MakeRandomBlock();
	}

	public void Restart() {
		Disable();
		SceneManager.LoadScene("Main");
	}

	public void StartGame() {
		StartSinglePlayer();
		return;
		CharacterComponent chc = gameObject.AddComponent<CharacterComponent>();
		chc.Character = (CharacterType)Utils.NextRandomColor();

		OpponentComponent oc = gameObject.AddComponent<OpponentComponent>();
		oc.Character = (CharacterType)Utils.NextRandomColor();
	}

	public void StartSinglePlayer() {
		CharacterComponent chc = gameObject.AddComponent<CharacterComponent>();
		chc.Character = (CharacterType)Utils.NextRandomColor();
	}
}
