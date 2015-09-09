using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ALPSGUI : MonoBehaviour {
	Font arialFont; 
	bool menuIsActive,configPanelIsActive;
	GameObject customConfigPanel,deviceSelectionPanel;
	GameObject[] horizontalSeparatorCustomConfigArray;
	GameObject[] buttonCustomConfigArray;
	GameObject[] buttonMinusCustomConfigArray;
	GameObject[] buttonPlusCustomConfigArray;
	GameObject[] textButtonCustomConfigArray;
	GameObject[] sliderCustomConfigArray;
	GameObject OptionBarrelDistortion;
	GameObject OptionChromaticCorrection;
	string[] settingArray = {"Width","Height","IPD","ILD","K1","K2","CC"};


	public static ALPSConfig deviceConfig;
	public static ALPSController controller;
	// Use this for initialization

	public static void AddComponents(GameObject _obj, params System.Type[] _types){
		foreach (System.Type type in _types) {
			_obj.AddComponent(type);
		}
	}

	public static void SetRectTransform (RectTransform _rect,Vector3 _anchoredPosition3D, Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _pivot,Vector2 _sizeDelta){
		_rect.anchoredPosition3D 	= _anchoredPosition3D;
		_rect.anchorMin 			= _anchorMin;
		_rect.anchorMax 			= _anchorMax;
		_rect.pivot 				= _pivot;
		_rect.sizeDelta 			= _sizeDelta;
	}


	public void Init () {

		//Measures
		int buttonHeightPix = (int)ALPSController.MmToPixel(10);
		int separatorHeightPix = 2;
		int moreButtonWidth = (int)ALPSController.MmToPixel(4);
		int maskHeight = ALPSController.screenHeightPix;
		int maskWidth = (int)ALPSController.MmToPixel(37);
		int scrollbarWidth = (int)ALPSController.MmToPixel(0.8f);
		int scrollRectWidth = maskWidth - scrollbarWidth;
		int scrollRectHeight = maskHeight-buttonHeightPix;
		int titleTextMarginLeft = (int)ALPSController.MmToPixel(2);
		int nextBackButtonWidth = (int)ALPSController.MmToPixel(6);
		int titleTextWidth = maskWidth - nextBackButtonWidth - separatorHeightPix - titleTextMarginLeft;
		int fontSize = (int)(scrollRectWidth * 0.07f);
		int fontSizeSmall = (int)(scrollRectWidth * 0.05f);

		int arrowImageWidth = (int)ALPSController.MmToPixel(3);
		int deviceIconMarginLeft = (int)ALPSController.MmToPixel(3.5f);
		int minusPlusButtonWidth = (int)ALPSController.MmToPixel(5.5f);
		int checkBoxWidth = (int)ALPSController.MmToPixel(4);
		int checkMarkWidth = (int)ALPSController.MmToPixel(2);
		int sliderHandleWidth = (int)ALPSController.MmToPixel(2.5f);
		int sliderWidth = (int)ALPSController.MmToPixel(22);
		int sliderMarginBottom = (int)ALPSController.MmToPixel(4);
		int sliderTextMarginTop = (int)ALPSController.MmToPixel(3);
		menuIsActive = false;
		configPanelIsActive = false;

		int UILayerIndex = LayerMask.NameToLayer ("Default");
		arialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");

		//This is the main canvas
		GameObject canvasGO = new GameObject("ALPSGUICanvas");
		canvasGO.layer = UILayerIndex;
		AddComponents (canvasGO,typeof(Canvas),typeof(GraphicRaycaster),typeof(EventSystem),typeof(StandaloneInputModule),typeof(TouchInputModule));
		Canvas canvas = canvasGO.GetComponent<Canvas>();

		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.pixelPerfect = true;
		canvas.sortingOrder = 100;

		//This is the mask for the menu
		GameObject menuMask = new GameObject("MenuMask");
		menuMask.layer = UILayerIndex;
		menuMask.transform.parent = canvasGO.transform;
		AddComponents(menuMask,typeof(CanvasRenderer),typeof(Image),typeof(Mask));
		SetRectTransform (menuMask.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (maskWidth,maskHeight));

		//This is the Sliding Panel (which contains the two tabs)
		GameObject horizSlidingPanel = new GameObject("SlidingPanel");
		horizSlidingPanel.layer = UILayerIndex;
		horizSlidingPanel.transform.parent = menuMask.transform;
		AddComponents(horizSlidingPanel,typeof(CanvasRenderer),typeof(Image));
		SetRectTransform (horizSlidingPanel.GetComponent<RectTransform> (), new Vector3 (0, 0, 0), new Vector2 (0, 0), new Vector2 (0, 0), new Vector2 (0, 0), new Vector2 (maskWidth * 2, maskHeight));


		//This is the more button
		GameObject imageMoreButton = new GameObject("MoreButton");
		imageMoreButton.layer = UILayerIndex;
		imageMoreButton.transform.parent = canvasGO.transform;
		AddComponents(imageMoreButton,typeof(Button),typeof(RectTransform),typeof(Image));
		SetRectTransform (imageMoreButton.GetComponent<RectTransform> (),new Vector3 (0,moreButtonWidth,0),new Vector2 (0.5f,0),new Vector2 (0.5f,0),new Vector2 (0.5f,0.5f),new Vector2 (moreButtonWidth,moreButtonWidth));
		imageMoreButton.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/more");

		imageMoreButton.GetComponent<Button>().onClick.AddListener(() => { 
			menuIsActive = !menuIsActive;
			menuMask.SetActive (menuIsActive);
		}); 

		/******************************************************************************************/
		//
		//
		//	DEVICE SELECTION PANEL
		//
		//
		//
		/******************************************************************************************/

		//This is the DeviceSelection Panel
		deviceSelectionPanel = new GameObject("DeviceSelectionPanel");
		deviceSelectionPanel.layer = UILayerIndex;
		deviceSelectionPanel.transform.parent = horizSlidingPanel.transform;
		AddComponents(deviceSelectionPanel,typeof(CanvasRenderer),typeof(Image));
		SetRectTransform (deviceSelectionPanel.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (maskWidth,maskHeight));

		//ScrollRect of the DeviceSelection Panel
		GameObject scrollRectDeviceSelection = new GameObject("ScrollRectDeviceSelection");
		scrollRectDeviceSelection.layer = UILayerIndex;
		scrollRectDeviceSelection.transform.parent = deviceSelectionPanel.transform;
		AddComponents(scrollRectDeviceSelection,typeof(CanvasRenderer),typeof(Image),typeof(Mask));
		SetRectTransform (scrollRectDeviceSelection.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (scrollRectWidth,scrollRectHeight));
		scrollRectDeviceSelection.AddComponent (typeof(ScrollRect));
		scrollRectDeviceSelection.GetComponent<ScrollRect> ().horizontal = false;
		scrollRectDeviceSelection.GetComponent<ScrollRect> ().vertical = true;


		//Content of the DeviceSelection Panel
		GameObject contentDeviceSelection = new GameObject("ContentDeviceSelection");
		contentDeviceSelection.layer = UILayerIndex;
		contentDeviceSelection.transform.parent = scrollRectDeviceSelection.transform;
		contentDeviceSelection.AddComponent (typeof(RectTransform));

		//set content RectTransform after adding buttons

		//Adding content to ScrollRect
		scrollRectDeviceSelection.GetComponent<ScrollRect> ().content = contentDeviceSelection.GetComponent<RectTransform> ();

		/*--------------*/
		//List of buttons
		Device[] deviceArray = (Device[])System.Enum.GetValues (typeof(Device));
		GameObject[] buttonArray = new GameObject[deviceArray.Length];
		GameObject[] textButtonArray = new GameObject[deviceArray.Length];
		GameObject[] horizontalSeparatorArray = new GameObject[deviceArray.Length-1];
		int YOffset = 0;
		for (int i =0; i<deviceArray.Length;i++) {
			Device dev = deviceArray[i];
			//separator
			if(i>0){
				GameObject separator = new GameObject ("Separator"+i);
				separator.layer = UILayerIndex;
				separator.transform.parent = contentDeviceSelection.transform;
				AddComponents (separator,typeof(CanvasRenderer),typeof(Image));
				SetRectTransform (separator.GetComponent<RectTransform> (),new Vector3 (0,-(separatorHeightPix+YOffset),0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (scrollRectWidth,separatorHeightPix));
				separator.GetComponent<Image> ().color = new Color (0.88f, 0.88f, 0.88f);
				YOffset += separatorHeightPix;
				horizontalSeparatorArray[i-1] = separator;
			}

			//button
			GameObject button1 = new GameObject ("Button"+deviceArray[i].ToString());
			button1.layer = UILayerIndex;
			button1.transform.parent = contentDeviceSelection.transform;
			AddComponents (button1,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image),typeof(Button));
			SetRectTransform (button1.GetComponent<RectTransform> (),new Vector3 (0,-(buttonHeightPix+YOffset),0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (scrollRectWidth,buttonHeightPix));
			YOffset += buttonHeightPix;
			ColorBlock cb = button1.GetComponent<Button>().colors;
			cb.highlightedColor = new Color (0.94f, 0.94f, 0.94f);
			button1.GetComponent<Button>().colors = cb;
			button1.GetComponent<Button>().onClick.AddListener(() => { 
				controller.SetDevice(dev);
				updateConfigText ();
			}); 

			//text
			GameObject textButton1 = new GameObject ("textButton"+deviceArray[i]);
			textButton1.layer = UILayerIndex;
			textButton1.transform.parent = button1.transform;
			AddComponents (textButton1,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));
			textButton1.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
			textButton1.GetComponent<Text> ().text = deviceArray[i].ToStringReadable();
			textButton1.GetComponent<Text> ().font = arialFont;
			textButton1.GetComponent<Text> ().color = new Color (0.5f, 0.5f, 0.5f);
			textButton1.GetComponent<Text> ().fontSize = fontSize;
			SetRectTransform (textButton1.GetComponent<RectTransform> (),new Vector3 ((deviceIconMarginLeft*2+arrowImageWidth),-buttonHeightPix*0.5f,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0.5f),new Vector2 (scrollRectWidth-(deviceIconMarginLeft*2+arrowImageWidth),buttonHeightPix));


			GameObject imageButton = new GameObject ("imageButton"+deviceArray[i]);
			imageButton.layer = UILayerIndex;
			imageButton.transform.parent = button1.transform;
			AddComponents (imageButton,typeof(CanvasRenderer),typeof(Image));
			imageButton.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/"+deviceArray[i]);
			imageButton.GetComponent<Image> ().color = new Color (0.09f, 0.63f, 0.52f);
			SetRectTransform (imageButton.GetComponent<RectTransform> (),new Vector3 (deviceIconMarginLeft,-buttonHeightPix*0.5f,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0.5f),new Vector2 (arrowImageWidth,arrowImageWidth));

			buttonArray[i] = button1;
			textButtonArray[i] = textButton1;

		}

		SetRectTransform (contentDeviceSelection.GetComponent<RectTransform> (),new Vector3 (0,-scrollRectHeight,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (scrollRectWidth,System.Math.Max(YOffset,scrollRectHeight)));

		//ScrollBar of the DeviceSelection Panel
		GameObject scrollBarDeviceSelection = new GameObject("ScrollBarDeviceSelection");
		scrollBarDeviceSelection.layer = UILayerIndex;
		scrollBarDeviceSelection.transform.parent = deviceSelectionPanel.transform;
		AddComponents (scrollBarDeviceSelection,typeof(CanvasRenderer),typeof(Image),typeof(Scrollbar));
		SetRectTransform (scrollBarDeviceSelection.GetComponent<RectTransform> (),new Vector3 (-scrollbarWidth,0,0),new Vector2 (1,0),new Vector2 (1,0),new Vector2 (0,0),new Vector2 (scrollbarWidth,scrollRectHeight));

		//Adding vertical ScrollBar to ScrollRect
		scrollRectDeviceSelection.GetComponent<ScrollRect> ().verticalScrollbar = scrollBarDeviceSelection.GetComponent<Scrollbar> ();

		//SlidingArea of the ScrollBar
		GameObject slindingAreaDeviceSelection = new GameObject("SlidingAreaDeviceSelection");
		slindingAreaDeviceSelection.layer = UILayerIndex;
		slindingAreaDeviceSelection.transform.parent = scrollBarDeviceSelection.transform;
		slindingAreaDeviceSelection.AddComponent (typeof(RectTransform));


		//Handle of the SlidingArea
		GameObject handleDeviceSelection = new GameObject("HandleDeviceSelection");
		handleDeviceSelection.layer = UILayerIndex;
		handleDeviceSelection.transform.parent = slindingAreaDeviceSelection.transform;
		AddComponents (handleDeviceSelection,typeof(CanvasRenderer),typeof(Image));
		handleDeviceSelection.GetComponent<Image> ().color = new Color (0.09f, 0.63f, 0.52f);

		scrollBarDeviceSelection.GetComponent<Scrollbar> ().targetGraphic = handleDeviceSelection.GetComponent<Image> ();
		scrollBarDeviceSelection.GetComponent<Scrollbar> ().handleRect = handleDeviceSelection.GetComponent<RectTransform> ();
		scrollBarDeviceSelection.GetComponent<Scrollbar> ().direction = Scrollbar.Direction.BottomToTop;
		scrollBarDeviceSelection.GetComponent<Scrollbar> ().size = 0;

		SetRectTransform (slindingAreaDeviceSelection.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (scrollbarWidth,scrollRectHeight));

		handleDeviceSelection.GetComponent<RectTransform> ().offsetMin = new Vector2(0, 0);
		handleDeviceSelection.GetComponent<RectTransform> ().offsetMax = new Vector2(0, 0);
		handleDeviceSelection.GetComponent<RectTransform> ().anchorMin = new Vector2 (0,0);
		handleDeviceSelection.GetComponent<RectTransform> ().anchorMax = new Vector2 (1,1);
		handleDeviceSelection.GetComponent<RectTransform> ().pivot = new Vector2 (0,0);



		//Pannel title of the DeviceSelection
		GameObject titleDeviceSelection = new GameObject("TitleDeviceSelection");
		titleDeviceSelection.layer = UILayerIndex;
		titleDeviceSelection.transform.parent = deviceSelectionPanel.transform;
		AddComponents (titleDeviceSelection,typeof(CanvasRenderer),typeof(Image));
		SetRectTransform (titleDeviceSelection.GetComponent<RectTransform> (),new Vector3 (0,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (maskWidth,buttonHeightPix));

		titleDeviceSelection.GetComponent<Image> ().color = new Color (0.13f,0.17f,0.22f);

		//Text of the title of the DeviceSelection
		GameObject textTitleDeviceSelection = new GameObject("TextTitleDeviceSelection");
		textTitleDeviceSelection.layer = UILayerIndex;
		textTitleDeviceSelection.transform.parent = titleDeviceSelection.transform;
		AddComponents(textTitleDeviceSelection,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));
		textTitleDeviceSelection.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
		textTitleDeviceSelection.GetComponent<Text> ().text = "Select a device";
		textTitleDeviceSelection.GetComponent<Text> ().color = Color.white;
		textTitleDeviceSelection.GetComponent<Text> ().fontSize = fontSize;
		textTitleDeviceSelection.GetComponent<Text> ().font = arialFont;
		SetRectTransform (textTitleDeviceSelection.GetComponent<RectTransform> (),new Vector3 (titleTextMarginLeft,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (titleTextWidth,buttonHeightPix));

		//Separator
		GameObject titleSeparatorDeviceSelection = new GameObject("TitleSeparatorDeviceSelection");
		titleSeparatorDeviceSelection.layer = UILayerIndex;
		titleSeparatorDeviceSelection.transform.parent = titleDeviceSelection.transform;
		AddComponents (titleSeparatorDeviceSelection,typeof(CanvasRenderer),typeof(Image));
		titleSeparatorDeviceSelection.GetComponent<Image> ().color = new Color (1f,1f,1f,0.39f);
		SetRectTransform (titleSeparatorDeviceSelection.GetComponent<RectTransform> (),new Vector3 (titleTextMarginLeft+titleTextWidth,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (2,buttonHeightPix));

		//Next button
		GameObject nextButton = new GameObject("NextButton");
		nextButton.layer = UILayerIndex;
		nextButton.transform.parent = titleDeviceSelection.transform;
		AddComponents (nextButton,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image),typeof(Button));
		SetRectTransform (nextButton.GetComponent<RectTransform> (),new Vector3 (titleTextMarginLeft+titleTextWidth+separatorHeightPix,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (nextBackButtonWidth,buttonHeightPix));
		nextButton.GetComponent<Image> ().color = new Color (0.13f,0.17f,0.22f);

		GameObject imageNextButton = new GameObject("ImageNextButton");
		imageNextButton.layer = UILayerIndex;
		imageNextButton.transform.parent = nextButton.transform;
		imageNextButton.AddComponent(typeof(Image));
		SetRectTransform (imageNextButton.GetComponent<RectTransform> (),new Vector3 (nextBackButtonWidth*0.5f,buttonHeightPix*0.5f,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0.5f,0.5f),new Vector2 (arrowImageWidth,arrowImageWidth));
		imageNextButton.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/arrow_right");


		nextButton.GetComponent<Button>().onClick.AddListener(() => { 
			switchPanel();
		}); 
		/******************************************************************************************/
		//****************************************************************************************/
		//****************************************************************************************/
		//	CUSTOM CONFIG PANEL
		//****************************************************************************************/
		//****************************************************************************************/
		//****************************************************************************************/
		/******************************************************************************************/

		//This is the CustomConfig Panel
		customConfigPanel = new GameObject("CustomConfigPanel");
		customConfigPanel.layer = UILayerIndex;
		customConfigPanel.transform.parent = horizSlidingPanel.transform;
		AddComponents (customConfigPanel,typeof(CanvasRenderer),typeof(Image));
		SetRectTransform (customConfigPanel.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (maskWidth,maskHeight));
		customConfigPanel.SetActive (configPanelIsActive);

		//ScrollRect of the CustomConfig Panel
		GameObject scrollRectCustomConfig = new GameObject("ScrollRectCustomConfig");
		scrollRectCustomConfig.layer = UILayerIndex;
		scrollRectCustomConfig.transform.parent = customConfigPanel.transform;
		AddComponents (scrollRectCustomConfig,typeof(CanvasRenderer),typeof(Image),typeof(Mask),typeof(ScrollRect));
		SetRectTransform (scrollRectCustomConfig.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (scrollRectWidth,scrollRectHeight));
		scrollRectCustomConfig.GetComponent<ScrollRect> ().horizontal = false;
		scrollRectCustomConfig.GetComponent<ScrollRect> ().vertical = true;
		
		
		//Content of the CustomConfig Panel
		GameObject contentCustomConfig = new GameObject("ContentCustomConfig");
		contentCustomConfig.layer = UILayerIndex;
		contentCustomConfig.transform.parent = scrollRectCustomConfig.transform;
		contentCustomConfig.AddComponent (typeof(RectTransform));

		//Adding content to ScrollRect
		scrollRectCustomConfig.GetComponent<ScrollRect> ().content = contentCustomConfig.GetComponent<RectTransform> ();

		/*************************************************
		 * 
		 * Content
		 * 
		 *************************************************/

		GameObject toggleGroupSizeCustomConfig = new GameObject("ToggleGroupSizeCustomConfig");
		toggleGroupSizeCustomConfig.layer = UILayerIndex;
		toggleGroupSizeCustomConfig.transform.parent = contentCustomConfig.transform;
		AddComponents(toggleGroupSizeCustomConfig,typeof(RectTransform),typeof(ToggleGroup));
		SetRectTransform (toggleGroupSizeCustomConfig.GetComponent<RectTransform> (),new Vector3 (0,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (scrollRectWidth,buttonHeightPix));

		OptionBarrelDistortion = null;
		OptionChromaticCorrection = null;

		for(int i=0;i<2;i++){
			GameObject optionButton = new GameObject((i==0)?"OptionBarrelDistortion":"OptionChromaticCorrection");
			optionButton.layer = UILayerIndex;
			optionButton.transform.parent = toggleGroupSizeCustomConfig.transform;
			AddComponents(optionButton,typeof(RectTransform),typeof(Toggle));
			SetRectTransform (optionButton.GetComponent<RectTransform> (),new Vector3 (i*scrollRectWidth*0.5f,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (scrollRectWidth*0.5f,buttonHeightPix));



			//Don't forget to tell wether it is on or not
			optionButton.GetComponent<Toggle> ().isOn = true;

			//checkbox
			GameObject checkBox = new GameObject("CheckBox"+((i==0)?"BarrelDistortion":"ChromaticCorrection"));
			checkBox.layer = UILayerIndex;
			checkBox.transform.parent = optionButton.transform;
			AddComponents(checkBox,typeof(CanvasRenderer),typeof(Image));
			SetRectTransform (checkBox.GetComponent<RectTransform> (),new Vector3 (checkMarkWidth,-buttonHeightPix*0.5f,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0.5f),new Vector2 (checkBoxWidth,checkBoxWidth));
			checkBox.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/checkbox");

			//checkmark
			GameObject checkMark = new GameObject("CheckMark"+((i==0)?"BarrelDistortion":"ChromaticCorrection"));
			checkMark.layer = UILayerIndex;
			checkMark.transform.parent = checkBox.transform;
			AddComponents(checkMark,typeof(CanvasRenderer),typeof(Image));
			SetRectTransform (checkMark.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (checkMarkWidth,checkMarkWidth));
			checkMark.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/checkmark");

			//Text of the title of the DeviceSelection
			GameObject textCheckBox = new GameObject("TextCheckBox");
			textCheckBox.layer = UILayerIndex;
			textCheckBox.transform.parent = optionButton.transform;
			AddComponents(textCheckBox,typeof(RectTransform),typeof(Image));
			textCheckBox.GetComponent<Image> ().sprite = Resources.Load <Sprite> ((i==0)?"Sprites/barrel":"Sprites/cc");
			SetRectTransform (textCheckBox.GetComponent<RectTransform> (),new Vector3 (checkMarkWidth*2+checkBoxWidth,-buttonHeightPix*0.5f,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0.5f),new Vector2 (checkBoxWidth,checkBoxWidth));

			//Don't forget to assign target graphic
			optionButton.GetComponent<Toggle> ().targetGraphic = checkBox.GetComponent<Image> ();
			//Don't forget to assign graphic
			optionButton.GetComponent<Toggle> ().graphic = checkMark.GetComponent<Image> ();
			//Don't forget to assign toggle group
			//optionButton.GetComponent<Toggle> ().group = toggleGroupSizeCustomConfig.GetComponent<ToggleGroup> ();

			if(i==0){
				OptionBarrelDistortion = optionButton;
			}else{
				OptionChromaticCorrection = optionButton;
			}
		}

		OptionBarrelDistortion.GetComponent<Toggle> ().onValueChanged.AddListener ( (value) => {   
			controller.SetBarrelDistortionActive(value);
		});

		OptionChromaticCorrection.GetComponent<Toggle> ().onValueChanged.AddListener ( (value) => {   
			controller.SetChromaticCorrectionActive(value);
		});

		//Buttons
		int YOffsetCustomConfig = buttonHeightPix;
		horizontalSeparatorCustomConfigArray = new GameObject[7];
		buttonCustomConfigArray = new GameObject[7];
		buttonMinusCustomConfigArray = new GameObject[7];
		buttonPlusCustomConfigArray = new GameObject[7];
		textButtonCustomConfigArray = new GameObject[7];
		sliderCustomConfigArray = new GameObject[7];

		for (int i = 0; i<7; i++) {
			//separator
			GameObject separator = new GameObject ("SeparatorCustomConfig"+i);
			separator.layer = UILayerIndex;
			separator.transform.parent = contentCustomConfig.transform;
			AddComponents (separator,typeof(CanvasRenderer),typeof(Image));
			SetRectTransform (separator.GetComponent<RectTransform> (),new Vector3 (0,-(separatorHeightPix+YOffsetCustomConfig),0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (scrollRectWidth,separatorHeightPix));
			separator.GetComponent<Image> ().color = new Color (0.88f, 0.88f, 0.88f);
			YOffsetCustomConfig += separatorHeightPix;
			horizontalSeparatorCustomConfigArray[i] = separator;

			//button
			GameObject button1 = new GameObject ("Button"+settingArray[i]);
			button1.layer = UILayerIndex;
			button1.transform.parent = contentCustomConfig.transform;
			button1.AddComponent(typeof(RectTransform));
			SetRectTransform (button1.GetComponent<RectTransform> (),new Vector3 (0,-(buttonHeightPix+YOffsetCustomConfig),0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (scrollRectWidth,buttonHeightPix));
			YOffsetCustomConfig += buttonHeightPix;
			buttonCustomConfigArray[i] = button1;

			//text
			GameObject textButton1 = new GameObject ("textButton"+settingArray[i]);
			textButton1.layer = UILayerIndex;
			textButton1.transform.parent = button1.transform;
			AddComponents (textButton1,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));
			textButton1.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			textButton1.GetComponent<Text> ().text = settingArray[i];
			textButton1.GetComponent<Text> ().font = arialFont;
			textButton1.GetComponent<Text> ().color = new Color (0.5f, 0.5f, 0.5f);
			textButton1.GetComponent<Text> ().fontSize = fontSizeSmall;
			SetRectTransform (textButton1.GetComponent<RectTransform> (),new Vector3 (0,sliderTextMarginTop,0),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (scrollRectWidth-2*minusPlusButtonWidth,buttonHeightPix));
			textButtonCustomConfigArray[i] = textButton1;

			//buttonMinus and buttonPlus
			for(int j =0;j<2;j++){
				string sign = (j==0)?"Minus":"Plus";
				GameObject buttonMinusPlus = new GameObject ("Button"+sign+settingArray[i]);
				buttonMinusPlus.layer = UILayerIndex;
				buttonMinusPlus.transform.parent = button1.transform;
				AddComponents (buttonMinusPlus,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image),typeof(Button));
				SetRectTransform (buttonMinusPlus.GetComponent<RectTransform> (),new Vector3 ((j==0)?0:-minusPlusButtonWidth,0,0),new Vector2 ((j==0)?0:1,0),new Vector2 ((j==0)?0:1,0),new Vector2 (0,0),new Vector2 (minusPlusButtonWidth,buttonHeightPix));
				buttonMinusPlus.GetComponent<Image> ().color = new Color(0.93f,0.93f,0.93f);
				
				//text
				GameObject textButtonMinusPlus = new GameObject ("textButton"+sign+settingArray[i]);
				textButtonMinusPlus.layer = UILayerIndex;
				textButtonMinusPlus.transform.parent = buttonMinusPlus.transform;
				AddComponents (textButtonMinusPlus,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));
				textButtonMinusPlus.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
				textButtonMinusPlus.GetComponent<Text> ().text = (j==0)?"-":"+";
				textButtonMinusPlus.GetComponent<Text> ().font = arialFont;
				textButtonMinusPlus.GetComponent<Text> ().color = new Color (0.5f, 0.5f, 0.5f);
				textButtonMinusPlus.GetComponent<Text> ().fontSize = fontSize;

				textButtonMinusPlus.GetComponent<RectTransform> ().offsetMin = new Vector2(0, 0);
				textButtonMinusPlus.GetComponent<RectTransform> ().offsetMax = new Vector2(0, 0);
				textButtonMinusPlus.GetComponent<RectTransform> ().anchorMin = new Vector2 (0,0);
				textButtonMinusPlus.GetComponent<RectTransform> ().anchorMax = new Vector2 (1,1);
				textButtonMinusPlus.GetComponent<RectTransform> ().pivot = new Vector2 (0.5f,0.5f);	


				if(j==0){
					buttonMinusCustomConfigArray[i]=buttonMinusPlus;
				}else{
					buttonPlusCustomConfigArray[i]=buttonMinusPlus;
				} 

			}


			//Slider
			GameObject slider = new GameObject ("Slider"+settingArray[i]);
			slider.layer = UILayerIndex;
			slider.transform.parent = button1.transform;
			AddComponents (slider,typeof(RectTransform),typeof(Slider));
			SetRectTransform (slider.GetComponent<RectTransform> (),new Vector3 (0,sliderMarginBottom,0),new Vector2 (0.5f,0),new Vector2 (0.5f,0),new Vector2 (0.5f,0.5f),new Vector2 (sliderWidth,sliderHandleWidth));
			sliderCustomConfigArray[i] = slider;

			//Slider background
			GameObject sliderBackground = new GameObject("SliderBackground"+settingArray[i]);
			sliderBackground.layer = UILayerIndex;
			sliderBackground.transform.parent = slider.transform;
			AddComponents(sliderBackground,typeof(CanvasRenderer),typeof(Image));
			SetRectTransform (sliderBackground.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (sliderWidth,scrollbarWidth));
			sliderBackground.GetComponent<Image> ().color = new Color32(199,199,199,255);
		
			//slider fill area
			GameObject sliderFillArea = new GameObject("SliderFillArea"+settingArray[i]);
			sliderFillArea.layer = UILayerIndex;
			sliderFillArea.transform.parent = slider.transform;
			sliderFillArea.AddComponent(typeof(RectTransform));
			SetRectTransform (sliderFillArea.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (sliderWidth,scrollbarWidth));

				//fill
				GameObject sliderFill = new GameObject("SliderFill"+settingArray[i]);
				sliderFill.layer = UILayerIndex;
				sliderFill.transform.parent = sliderFillArea.transform;
				AddComponents(sliderFill,typeof(CanvasRenderer),typeof(Image));
				sliderFill.GetComponent<RectTransform> ().offsetMin = new Vector2(0, 0);
				sliderFill.GetComponent<RectTransform> ().offsetMax = new Vector2(0, 0);
				sliderFill.GetComponent<RectTransform> ().anchorMin = new Vector2 (0,0);
				sliderFill.GetComponent<RectTransform> ().anchorMax = new Vector2 (0.5f,1);
				sliderFill.GetComponent<RectTransform> ().pivot = new Vector2 (0.5f,0.5f);
				sliderFill.GetComponent<Image> ().color = new Color32(22,160,133,255);

			//HandleSlideArea
			GameObject handleSlideArea = new GameObject("HandleSlideArea"+settingArray[i]);
			handleSlideArea.layer = UILayerIndex;
			handleSlideArea.transform.parent = slider.transform;
			handleSlideArea.AddComponent(typeof(RectTransform));
			SetRectTransform (handleSlideArea.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (sliderWidth,sliderHandleWidth));

				//handle
				GameObject handle = new GameObject("Handle"+settingArray[i]);
				handle.layer = UILayerIndex;
				handle.transform.parent = handleSlideArea.transform;
				AddComponents(handle,typeof(CanvasRenderer),typeof(Image));
			SetRectTransform (handle.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (0.5f,0.5f),new Vector2 (sliderHandleWidth,sliderHandleWidth));
				handle.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/knob");
				handle.GetComponent<Image> ().preserveAspect = true;

			//don't forget target graphic
			slider.GetComponent<Slider> ().targetGraphic = handle.GetComponent<Image> ();
			//don't forget fill rect
			slider.GetComponent<Slider> ().fillRect = sliderFill.GetComponent<RectTransform> ();
			//don't forget handle rect
			slider.GetComponent<Slider> ().handleRect = handle.GetComponent<RectTransform> ();
			//don't forget direction
			slider.GetComponent<Slider> ().direction = Slider.Direction.LeftToRight;

			slider.GetComponent<Slider> ().value = 0.5f;
		}

		//add listeners to minus and plus

		//Width
		buttonMinusCustomConfigArray[0].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToDeviceWidth(-1);
		});

		buttonPlusCustomConfigArray[0].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToDeviceWidth(1);
		});

		sliderCustomConfigArray [0].GetComponent<Slider> ().wholeNumbers = true;
		sliderCustomConfigArray [0].GetComponent<Slider> ().minValue = 0;
		sliderCustomConfigArray [0].GetComponent<Slider> ().maxValue = 300;
		sliderCustomConfigArray[0].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {  
			controller.SetDeviceWidth((int)value);
		});

		//Height
		buttonMinusCustomConfigArray[1].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToDeviceHeight(-1);
		});
		
		buttonPlusCustomConfigArray[1].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToDeviceHeight(1);
		});

		sliderCustomConfigArray [1].GetComponent<Slider> ().wholeNumbers = true;
		sliderCustomConfigArray [1].GetComponent<Slider> ().minValue = 0;
		sliderCustomConfigArray [1].GetComponent<Slider> ().maxValue = 300;
		sliderCustomConfigArray[1].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {   
			controller.SetDeviceHeight((int)value);
		});

		//IPD
		buttonMinusCustomConfigArray[2].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToIPD(-1);
		});
		
		buttonPlusCustomConfigArray[2].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToIPD(1);
		});

		sliderCustomConfigArray [2].GetComponent<Slider> ().wholeNumbers = true;
		sliderCustomConfigArray [2].GetComponent<Slider> ().minValue = -150;
		sliderCustomConfigArray [2].GetComponent<Slider> ().maxValue = 150;
		sliderCustomConfigArray[2].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {   
			controller.SetDeviceIPD((int)value);
		});

		//ILD
		buttonMinusCustomConfigArray[3].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToILD(-1);
		});
		
		buttonPlusCustomConfigArray[3].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToILD(1);
		});

		sliderCustomConfigArray [3].GetComponent<Slider> ().wholeNumbers = true;
		sliderCustomConfigArray [3].GetComponent<Slider> ().minValue = -150;
		sliderCustomConfigArray [3].GetComponent<Slider> ().maxValue = 150;
		sliderCustomConfigArray[3].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {   
			controller.SetDeviceILD((int)value);
		});

		//k1
		buttonMinusCustomConfigArray[4].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToK1(-0.1f);
		});
		
		buttonPlusCustomConfigArray[4].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToK1(0.1f);
		});

		sliderCustomConfigArray [4].GetComponent<Slider> ().wholeNumbers = false;
		sliderCustomConfigArray [4].GetComponent<Slider> ().minValue = -10;
		sliderCustomConfigArray [4].GetComponent<Slider> ().maxValue = 10;
		sliderCustomConfigArray[4].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {   
			controller.SetDeviceK1(value);
		});

		//k2
		buttonMinusCustomConfigArray[5].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToK2(-0.1f);
		});
		
		buttonPlusCustomConfigArray[5].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToK2(0.1f);
		});

		sliderCustomConfigArray [5].GetComponent<Slider> ().wholeNumbers = false;
		sliderCustomConfigArray [5].GetComponent<Slider> ().minValue = -10;
		sliderCustomConfigArray [5].GetComponent<Slider> ().maxValue = 10;
		sliderCustomConfigArray[5].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {   
			controller.SetDeviceK2(value);
		});

		//cc
		buttonMinusCustomConfigArray[6].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToChromaticCorrection(-0.1f);
		});
		
		buttonPlusCustomConfigArray[6].GetComponent<Button>().onClick.AddListener(() => { 
			controller.AddToChromaticCorrection(0.1f);
		});

		sliderCustomConfigArray [6].GetComponent<Slider> ().wholeNumbers = false;
		sliderCustomConfigArray [6].GetComponent<Slider> ().minValue = -5;
		sliderCustomConfigArray [6].GetComponent<Slider> ().maxValue = 5;
		sliderCustomConfigArray[6].GetComponent<Slider> ().onValueChanged.AddListener ( (value) => {   
			controller.SetDeviceCC(value);
		});

		SetRectTransform (contentCustomConfig.GetComponent<RectTransform> (),new Vector3 (0,-scrollRectHeight,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (scrollRectWidth,System.Math.Max(YOffsetCustomConfig,scrollRectHeight)));


		/*************************************************
		 * 
		 * ScrollBar
		 * 
		 *************************************************/
		//ScrollBar of the CustomConfig Panel
		GameObject scrollBarCustomConfig = new GameObject("ScrollBarCustomConfig");
		scrollBarCustomConfig.layer = UILayerIndex;
		scrollBarCustomConfig.transform.parent = customConfigPanel.transform;
		AddComponents (scrollBarCustomConfig,typeof(CanvasRenderer),typeof(Image),typeof(Scrollbar));
		SetRectTransform (scrollBarCustomConfig.GetComponent<RectTransform> (),new Vector3 (-scrollbarWidth,0,0),new Vector2 (1,0),new Vector2 (1,0),new Vector2 (0,0),new Vector2 (scrollbarWidth,scrollRectHeight));
		
		//Adding vertical ScrollBar to ScrollRect
		scrollRectCustomConfig.GetComponent<ScrollRect> ().verticalScrollbar = scrollBarCustomConfig.GetComponent<Scrollbar> ();
		
		//SlidingArea of the ScrollBar
		GameObject slindingAreaCustomConfig = new GameObject("SlidingAreaCustomConfig");
		slindingAreaCustomConfig.layer = UILayerIndex;
		slindingAreaCustomConfig.transform.parent = scrollBarCustomConfig.transform;
		slindingAreaCustomConfig.AddComponent (typeof(RectTransform));
		
		
		//Handle of the SlidingArea
		GameObject handleCustomConfig = new GameObject("HandleCustomConfig");
		handleCustomConfig.layer = UILayerIndex;
		handleCustomConfig.transform.parent = slindingAreaCustomConfig.transform;
		AddComponents (handleCustomConfig,typeof(CanvasRenderer),typeof(Image));
		handleCustomConfig.GetComponent<Image> ().color = new Color (0.09f, 0.63f, 0.52f);
		
		scrollBarCustomConfig.GetComponent<Scrollbar> ().targetGraphic = handleCustomConfig.GetComponent<Image> ();
		scrollBarCustomConfig.GetComponent<Scrollbar> ().handleRect = handleCustomConfig.GetComponent<RectTransform> ();
		scrollBarCustomConfig.GetComponent<Scrollbar> ().direction = Scrollbar.Direction.BottomToTop;
		scrollBarCustomConfig.GetComponent<Scrollbar> ().size = 0;

		SetRectTransform (slindingAreaCustomConfig.GetComponent<RectTransform> (),new Vector3 (0,0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (scrollbarWidth,scrollRectHeight));
		
		handleCustomConfig.GetComponent<RectTransform> ().offsetMin = new Vector2(0, 0);
		handleCustomConfig.GetComponent<RectTransform> ().offsetMax = new Vector2(0, 0);
		handleCustomConfig.GetComponent<RectTransform> ().anchorMin = new Vector2 (0,0);
		handleCustomConfig.GetComponent<RectTransform> ().anchorMax = new Vector2 (1,1);
		handleCustomConfig.GetComponent<RectTransform> ().pivot = new Vector2 (0,0);

		//Pannel title of the DeviceSelection
		GameObject titleCustomConfig = new GameObject("TitleCustomConfig");
		titleCustomConfig.layer = UILayerIndex;
		titleCustomConfig.transform.parent = customConfigPanel.transform;
		AddComponents (titleCustomConfig,typeof(CanvasRenderer),typeof(Image));
		SetRectTransform (titleCustomConfig.GetComponent<RectTransform> (),new Vector3 (0,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (maskWidth,buttonHeightPix));

		titleCustomConfig.GetComponent<Image> ().color = new Color (0.13f,0.17f,0.22f);


		/*************************************************
		 * 
		 * Title
		 * 
		 *************************************************/

		//Text of the title of the DeviceSelection
		GameObject textTitleCustomConfig = new GameObject("TextTitleCustomConfig");
		textTitleCustomConfig.layer = UILayerIndex;
		textTitleCustomConfig.transform.parent = titleCustomConfig.transform;
		AddComponents (textTitleCustomConfig,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));
		textTitleCustomConfig.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
		textTitleCustomConfig.GetComponent<Text> ().text = "Custom configuration";
		textTitleCustomConfig.GetComponent<Text> ().color = Color.white;
		textTitleCustomConfig.GetComponent<Text> ().fontSize = fontSize;
		textTitleCustomConfig.GetComponent<Text> ().font = arialFont;
		SetRectTransform (textTitleCustomConfig.GetComponent<RectTransform> (),new Vector3 (maskWidth-titleTextWidth,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (titleTextWidth,buttonHeightPix));

		//Separator
		GameObject titleSeparatorCustomConfig = new GameObject("TitleSeparatorCustomConfig");
		titleSeparatorCustomConfig.layer = UILayerIndex;
		titleSeparatorCustomConfig.transform.parent = titleCustomConfig.transform;
		AddComponents(titleSeparatorCustomConfig,typeof(CanvasRenderer),typeof(Image));
		titleSeparatorCustomConfig.GetComponent<Image> ().color = new Color (1f,1f,1f,0.39f);
		SetRectTransform (titleSeparatorCustomConfig.GetComponent<RectTransform> (),new Vector3 (nextBackButtonWidth,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (separatorHeightPix,buttonHeightPix));
		
		//Back button
		GameObject backButton = new GameObject("BackButton");
		backButton.layer = UILayerIndex;
		backButton.transform.parent = titleCustomConfig.transform;
		AddComponents (backButton,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image),typeof(Button));
		SetRectTransform (backButton.GetComponent<RectTransform> (),new Vector3 (0,-buttonHeightPix,0),new Vector2 (0,1),new Vector2 (0,1),new Vector2 (0,0),new Vector2 (nextBackButtonWidth,buttonHeightPix));
		backButton.GetComponent<Image> ().color = new Color (0.13f,0.17f,0.22f);

		backButton.GetComponent<Button>().onClick.AddListener(() => { 
			switchPanel();
		});

		//image Back button
		GameObject imageBackButton = new GameObject("ImageBackButton");
		imageBackButton.layer = UILayerIndex;
		imageBackButton.transform.parent = backButton.transform;
		imageBackButton.AddComponent(typeof(Image));
		SetRectTransform (imageBackButton.GetComponent<RectTransform> (),new Vector3 (nextBackButtonWidth*0.5f,buttonHeightPix*0.5f,0),new Vector2 (0,0),new Vector2 (0,0),new Vector2 (0.5f,0.5f),new Vector2 (arrowImageWidth,arrowImageWidth));
		imageBackButton.GetComponent<RectTransform> ().Rotate(0,0,180);
		imageBackButton.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("Sprites/arrow_right");


		menuMask.SetActive (menuIsActive);
		updateConfigText ();
	}

	public void switchPanel(){
		deviceSelectionPanel.SetActive(configPanelIsActive);
		configPanelIsActive = !configPanelIsActive;
		customConfigPanel.SetActive(configPanelIsActive);
	}

	public void updateConfigText(){
		textButtonCustomConfigArray[0].GetComponent<Text> ().text = settingArray[0]+" : "+(int)deviceConfig.width+" mm";
		sliderCustomConfigArray[0].GetComponent<Slider> ().value = (int)deviceConfig.width;

		textButtonCustomConfigArray[1].GetComponent<Text> ().text = settingArray[1]+" : "+(int)deviceConfig.height+" mm";
		sliderCustomConfigArray[1].GetComponent<Slider> ().value = (int)deviceConfig.height;

		textButtonCustomConfigArray[2].GetComponent<Text> ().text = settingArray[2]+" : "+(int)deviceConfig.IPD+" mm";
		sliderCustomConfigArray[2].GetComponent<Slider> ().value = (int)deviceConfig.IPD;

		textButtonCustomConfigArray[3].GetComponent<Text> ().text = settingArray[3]+" : "+(int)deviceConfig.ILD+" mm";
		sliderCustomConfigArray[3].GetComponent<Slider> ().value = (int)deviceConfig.ILD;

		textButtonCustomConfigArray[4].GetComponent<Text> ().text = settingArray[4]+" : "+(float)deviceConfig.K1;
		sliderCustomConfigArray[4].GetComponent<Slider> ().value = deviceConfig.K1;

		textButtonCustomConfigArray[5].GetComponent<Text> ().text = settingArray[5]+" : "+(float)deviceConfig.K2;
		sliderCustomConfigArray[5].GetComponent<Slider> ().value = deviceConfig.K2;

		textButtonCustomConfigArray[6].GetComponent<Text> ().text = settingArray[6]+" : "+(float)deviceConfig.CC;
		sliderCustomConfigArray[6].GetComponent<Slider> ().value = deviceConfig.CC;
	
		OptionBarrelDistortion.GetComponent<Toggle> ().isOn = deviceConfig.barrelDistortionActive;
		OptionChromaticCorrection.GetComponent<Toggle> ().isOn = deviceConfig.chromaticCorrectionActive;
	}
}
