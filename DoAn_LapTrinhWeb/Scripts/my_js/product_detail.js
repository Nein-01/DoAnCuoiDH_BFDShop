$(document).ready(function() {
		    var slider = $("#slider");
		    var thumb = $("#thumb");
		    var slidesPerPage = 4; //globaly define number of elements per page
		    var syncedSecondary = true;
		    slider.owlCarousel({
		        items: 1,
				slideSpeed: 2000,
				nav: true,
				autoplay: false, 
		        dots: false,
				loop: true,
				navText: ['<svg  width="18px" height="18px" viewBox="0 0 11 20"><path style="fill:none;stroke-width: 1px;stroke: #5f5f5f;" class="svg_image" d="M9.554,1.001l-8.607,8.607l8.607,8.606"/></svg>', '<svg  width="25px" height="25px" viewBox="0 0 11 20" version="1.1"><path class="svg_image" style="fill:none;stroke-width: 1px;stroke: #5f5f5f;" d="M1.054,18.214l8.606,-8.606l-8.606,-8.607"/></svg>'],
				responsiveRefreshRate: 200,
		    }).on('changed.owl.carousel', syncPosition);
		    thumb
		        .on('initialized.owl.carousel', function() {
		            thumb.find(".owl-item").eq(0).addClass("current");
		        })
		        .owlCarousel({
		            items: slidesPerPage,
		            dots: false,
					nav: true,
					autoplay: false, 
					item: 4,
		            smartSpeed: 200,
		            slideSpeed: 500,
		            slideBy: slidesPerPage, 
		        	navText: ['<svg width="24px" height="24px" viewBox="0 0 24 24" version="1.1" xmlns="http://www.w3.org/2000/svg"><circle id="Oval-5" fill="#000000" opacity="0.5" cx="12" cy="12" r="10"></circle><path d="M6.96323356,15.1775211 C6.62849853,15.5122561 6.08578582,15.5122561 5.75105079,15.1775211 C5.41631576,14.842786 5.41631576,14.3000733 5.75105079,13.9653383 L10.8939067,8.82248234 C11.2184029,8.49798619 11.7409054,8.4866328 12.0791905,8.79672747 L17.2220465,13.5110121 C17.5710056,13.8308912 17.5945795,14.3730917 17.2747004,14.7220508 C16.9548212,15.0710098 16.4126207,15.0945838 16.0636617,14.7747046 L11.5257773,10.6149773 L6.96323356,15.1775211 Z" id="Path-94" fill="#fff" fill-rule="nonzero" transform="translate(11.500001, 12.000001) scale(-1, 1) rotate(-270.000000) translate(-11.500001, -12.000001) "></path></svg>', '<svg width="24px" height="24px" viewBox="0 0 24 24" version="1.1" xmlns="http://www.w3.org/2000/svg"><circle id="Oval-5" fill="#000000" opacity="0.5" cx="12" cy="12" r="10"></circle><path d="M7.96323356,15.1775211 C7.62849853,15.5122561 7.08578582,15.5122561 6.75105079,15.1775211 C6.41631576,14.842786 6.41631576,14.3000733 6.75105079,13.9653383 L11.8939067,8.82248234 C12.2184029,8.49798619 12.7409054,8.4866328 13.0791905,8.79672747 L18.2220465,13.5110121 C18.5710056,13.8308912 18.5945795,14.3730917 18.2747004,14.7220508 C17.9548212,15.0710098 17.4126207,15.0945838 17.0636617,14.7747046 L12.5257773,10.6149773 L7.96323356,15.1775211 Z" id="Path-94" fill="#fff" fill-rule="nonzero" transform="translate(12.500001, 12.000001) rotate(-270.000000) translate(-12.500001, -12.000001) "></path></svg>'],
		            responsiveRefreshRate: 100
		        }).on('changed.owl.carousel', syncPosition2);
		    function syncPosition(el) {
		        var count = el.item.count - 1;
		        var current = Math.round(el.item.index - (el.item.count / 2) - .5);
		        if (current < 0) {
		            current = count;
		        }
		        if (current > count) {
		            current = 0;
		        }
		        thumb
		            .find(".owl-item")
		            .removeClass("current")
		            .eq(current)
		            .addClass("current");
		        var onscreen = thumb.find('.owl-item.active').length - 1;
		        var start = thumb.find('.owl-item.active').first().index();
		        var end = thumb.find('.owl-item.active').last().index();
		        if (current > end) {
		            thumb.data('owl.carousel').to(current, 100, true);
		        }
		        if (current < start) {
		            thumb.data('owl.carousel').to(current - onscreen, 100, true);
		        }
		    }
		    function syncPosition2(el) {
		        if (syncedSecondary) {
		            var number = el.item.index;
		            slider.data('owl.carousel').to(number, 100, true);
		        }
		    }
		    thumb.on("click", ".owl-item", function(e) {
		        e.preventDefault();
		        var number = $(this).index();
		        slider.data('owl.carousel').to(number, 300, true);
		    });


            $(".qtyminus").on("click",function(){
                var now = $(".qty").val();
                if ($.isNumeric(now)){
                    if (parseInt(now) -1> 0)
                    { now--;}
                    $(".qty").val(now);
                }
            })            
            $(".qtyplus").on("click",function(){
                var now = $(".qty").val();
                if ($.isNumeric(now)){
                    $(".qty").val(parseInt(now)+1);
                }
            });
		});