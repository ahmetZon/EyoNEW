jQuery(document).ready(function ($) {
	var element = jQuery('.octf_tools_bar__icon_side_bar'),
		sidebar = jQuery('.octf_tools_bar'),
		isLoaded = 0;

	jQuery('.octf_tools_bar').fadeIn("slow");

	function toolbar_handler() {
		var isActive = !element.hasClass('active');
		lazyLoad();

		element.toggleClass('active', isActive);
		sidebar.toggleClass('active', isActive);
		jQuery('body').toggleClass('active_tools_bar_sidebar', isActive);
	}

	function lazyLoad() {
		if (0 !== isLoaded) return;
		isLoaded = -1;
		var imgs = jQuery('img[data-src]', sidebar);
		if (imgs && imgs.length) {
			var all_img = imgs.length;
			imgs.each(function (key, img) {
				img.setAttribute('src', img.getAttribute('data-src'));
				img.removeAttribute('data-src');
				img.addEventListener('load', function () {
					--all_img;
					if (!all_img) {
						isLoaded = 1;
						jQuery('.octf_tools_bar').addClass('loaded');
					}
				})
			})
		}
	}

	jQuery('.octf_tools_bar__icon_side_bar,.octf_tools_bar__sidebar-cover').on('click', toolbar_handler);
	jQuery(sidebar).on('swiperight', toolbar_handler);
});


 
 