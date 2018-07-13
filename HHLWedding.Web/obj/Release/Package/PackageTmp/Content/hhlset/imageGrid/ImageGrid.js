(function ($) {

    //弹出层加载
    $.fn.imagesGrid = function (option, event) {

        var cfg = $.extend({}, $.fn.imagesGrid.defaults, option);
        cfg.element = $(this);
        this._imgGrid = new ImagesGridModal(cfg);
        this._imgGrid.open(event);

    };

    // 弹出层关闭
    $.fn.imagesGrid.defaults = {
        onModalOpen: $.noop,
        onModalClose: $.noop,
    };

    //弹出层图片
    function ImagesGridModal(cfg) {

        this.images = cfg.images;
        this.imageIndex = null;

        this.$modal = null;
        this.$indicator = null;
        this.$document = $(document);

        this.open = function (imageIndex) {

            if (this.$modal && this.$modal.is(':visible')) {
                return;
            }

            this.imageIndex = parseInt(imageIndex) || 0;

            this.render();

        };

        this.close = function (event) {

            if (!this.$modal) {
                return;
            }

            this.$modal.animate({
                opacity: 0
            }, {
                duration: 100,
                complete: function () {

                    this.$modal.remove();
                    this.$modal = null;
                    this.$indicator = null;
                    this.imageIndex = null;

                    cfg.onModalClose();

                }.bind(this)
            });

            this.$document.off('keyup', this.keyUp);

        };

        this.render = function () {

            this.renderModal();
            this.renderCaption();
            this.renderCloseButton();
            this.renderInnerContainer();
            this.renderIndicatorContainer();

            this.keyUp = this.keyUp.bind(this);
            this.$document.on('keyup', this.keyUp);

            var self = this;

            this.$modal.animate({
                opacity: 1
            }, {
                duration: 100,
                complete: function () {
                    cfg.onModalOpen(self.$modal);
                }
            });

        };

        this.renderModal = function () {
            this.$modal = $('<div>', {
                class: 'imgs-grid-modal'
            }).appendTo('body');
        };

        this.renderCaption = function () {

            this.$caption = $('<div>', {
                class: 'modal-caption',
                text: this.getImageCaption(this.imageIndex)
            }).appendTo(this.$modal);

        };

        this.renderCloseButton = function () {
            this.$modal.append($('<div>', {
                class: 'modal-close',
                click: this.close.bind(this)
            }));
        };

        this.renderInnerContainer = function () {

            var image = this.getImage(this.imageIndex),
                self = this;

            this.$modal.append(
                $('<div>', {
                    class: 'modal-inner'
                }).append(
                    $('<div>', {
                        class: 'modal-image'
                    }).append(
                        $('<img>', {
                            src: image.src,
                            alt: image.alt,
                            title: image.title,
                            click: function (event) {
                                self.imageClick(event, $(this), image);
                            }
                        })
                    ),
                    $('<div>', {
                        class: 'modal-control left',
                        click: this.prev.bind(this)
                    }).append(
                        $('<div>', {
                            class: 'arrow left'
                        })
                    ),
                    $('<div>', {
                        class: 'modal-control right',
                        click: this.next.bind(this)
                    }).append(
                        $('<div>', {
                            class: 'arrow right'
                        })
                    )
                )
            );

            if (this.images.length <= 1) {
                this.$modal.find('.modal-control').hide();
            }

        };

        this.renderIndicatorContainer = function () {

            if (this.images.length == 1) {
                return;
            }

            this.$indicator = $('<div>', {
                class: 'modal-indicator'
            });

            var list = $('<ul>');

            for (var i = 0; i < this.images.length; ++i) {
                list.append($('<li>', {
                    class: this.imageIndex == i ? 'selected' : '',
                    click: this.indicatorClick.bind(this),
                    data: { index: i }
                }));
            }

            this.$indicator.append(list);

            this.$modal.append(this.$indicator);

        };

        this.prev = function () {
            if (this.imageIndex > 0) {
                --this.imageIndex;
            } else {
                this.imageIndex = this.images.length - 1;
            }
            this.updateImage();
        };

        this.next = function () {
            if (this.imageIndex < this.images.length - 1) {
                ++this.imageIndex;
            } else {
                this.imageIndex = 0;
            }
            this.updateImage();
        };

        this.updateImage = function () {

            var image = this.getImage(this.imageIndex);

            this.$modal.find('.modal-image img').attr({
                src: image.src,
                alt: image.alt,
                title: image.title
            });

            this.$modal.find('.modal-caption').text(
                this.getImageCaption(this.imageIndex));

            if (this.$indicator) {
                var indicatorList = this.$indicator.find('ul');
                indicatorList.children().removeClass('selected');
                indicatorList.children().eq(this.imageIndex).addClass('selected');
            }

        };

        this.imageClick = function (event, imageEl, image) {

            if (cfg.nextOnClick) {
                this.next();
            }

            cfg.onModalImageClick(event, imageEl, image);

        };

        this.indicatorClick = function (event) {
            var index = $(event.target).data('index');
            this.imageIndex = index;
            this.updateImage();
        };

        this.keyUp = function (event) {
            if (this.$modal) {
                switch (event.keyCode) {
                    case 27: // Esc
                        this.close();
                        break;
                    case 37: // Left arrow
                        this.prev();
                        break;
                    case 39: // Right arrow
                        this.next();
                        break;
                }
            }
        };

        this.getImage = function (index) {
            var image = this.images[index];
            if ($.isPlainObject(image)) {
                return image;
            } else {
                return { src: image, alt: '', title: '' }
            }
        };

        this.getImageCaption = function (imgIndex) {
            var img = this.getImage(imgIndex);
            return img.caption || '';
        };

    }

})(jQuery);