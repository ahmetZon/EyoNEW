
var revapi96,
	tpj;	
(function() {			
	if (!/loaded|interactive|complete/.test(document.readyState)) document.addEventListener("DOMContentLoaded",onLoad); else onLoad();	
	function onLoad() {				
		if (tpj===undefined) { tpj = jQuery; if("off" == "on") tpj.noConflict();}
	if(tpj("#slider-video").revolution == undefined){
		revslider_showDoubleJqueryError("#slider-video");
	}else{
		revapi96 = tpj("#slider-video").show().revolution({
			sliderType:"standard",
			jsFileLocation:"revolution/js/",
			sliderLayout:"fullScreen",
			dottedOverlay:"none",
			delay:9000,
			revealer: {
				direction: "tlbr_skew",
				color: "#ffffff",
				duration: "1500",
				delay: "0",
				easing: "Power3.easeOut",
				spinner: "2",
				spinnerColor: "rgba(0,0,0,",
			},
			navigation: {
				keyboardNavigation:"off",
				keyboard_direction: "horizontal",
				mouseScrollNavigation:"off",
 							mouseScrollReverse:"default",
				onHoverStop:"off",
				arrows: {
					style:"uranus",
					enable:false,
					hide_onmobile:false,
					hide_onleave:false,
					tmp:'',
					left: {
						h_align:"left",
						v_align:"center",
						h_offset:20,
						v_offset:50
					},
					right: {
						h_align:"right",
						v_align:"center",
						h_offset:20,
						v_offset:50
					}
				}
			},
			responsiveLevels:[1240,1024,778,480],
			visibilityLevels:[1240,1024,778,480],
			gridwidth:[1240,1024,778,480],
			gridheight:[800,700,600,500],
			lazyType:"none",
			parallax: {
				type:"scroll",
				origo:"slidercenter",
				speed:400,
				speedbg:0,
				speedls:0,
				levels:[5,10,15,20,25,30,35,40,45,-10,-15,-20,-25,50,51,55],
			},
			shadow:0,
			spinner:"spinner5",
			stopLoop:"off",
			stopAfterLoops:-1,
			stopAtSlide:-1,
			shuffle:"off",
			autoHeight:"off",
			fullScreenOffsetContainer: ".site-header",
			hideThumbsOnMobile:"off",
			hideSliderAtLimit:0,
			hideCaptionAtLimit:0,
			hideAllCaptionAtLilmit:0,
			debugMode:false,
			fallbacks: {
				simplifyAll:"off",
				nextSlideOnWindowFocus:"off",
				disableFocusListener:false,
			}
		});
	}; /* END OF revapi call */
	
    RsRevealerAddOn(tpj, revapi96, "<div class='rsaddon-revealer-spinner rsaddon-revealer-spinner-2'><div class='rsaddon-revealer-2' style='border-top-color: {{color}}0.65); border-bottom-color: {{color}}0.15); border-left-color: {{color}}0.65); border-right-color: {{color}}0.15)'><\/div><\/div>");

                RsTypewriterAddOn(tpj, revapi96);

 }; /* END OF ON LOAD FUNCTION */
}()); /* END OF WRAPPING FUNCTION */
