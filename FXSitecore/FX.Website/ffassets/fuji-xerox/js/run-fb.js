(function (initializeFunction) {
    'use strict';

    initializeFunction();
    const customModules = {};
    const matchMedia = window.matchMedia('(max-width: 767px)');
    const ANIMATION_DURATION = 400;
    const FOCUSABLE = 'a[href], button, area[href], input, select, textarea, [tabindex]';
    const DISABLE_TAB_INDEX_CLASSNAME = 'js-disableTabIndex';
    const RESIZE_EVENT_NAME = 'windowResize';
    const SCROLL_EVENT_NAME = 'documentScroll';
    const RESIZE_EVENT = new CustomEvent(RESIZE_EVENT_NAME);
    const SCROLL_EVENT = new CustomEvent(SCROLL_EVENT_NAME);
    const DOCUMENT_ELEMENT = document.documentElement;
    const BODY = document.body;

    /**
     * Class that controls scrolling on/off
     * @constructor
     */
    function ScrollController() {
        this._scrollPositionTop = DOCUMENT_ELEMENT.scrollTop || BODY.scrollTop;
        this._timer = 0;
        this._isActive = false;
        this._scrollEvent = this.onScroll.bind(this);
    }

    ScrollController.prototype = {
        lock: function () {
            this._scrollPositionTop = DOCUMENT_ELEMENT.scrollTop || BODY.scrollTop;
            DOCUMENT_ELEMENT.classList.add('-fixed');
            DOCUMENT_ELEMENT.height = BODY.offsetHeight + 'px';
            DOCUMENT_ELEMENT.style.top = '-' + this._scrollPositionTop + 'px';
        },
        unlock: function () {
            DOCUMENT_ELEMENT.classList.remove('-fixed');
            DOCUMENT_ELEMENT.style.transform = '';
            DOCUMENT_ELEMENT.style.top = '';
            window.scrollTo(0, this._scrollPositionTop);
        },
        onScroll: function () {
            const self = this;

            if (this._timer) {
                clearTimeout(this._timer);
                this._timer = 0;
            }

            this._timer = setTimeout(function () {
                self._scrollPositionTop = DOCUMENT_ELEMENT.scrollTop || BODY.scrollTop;
                DOCUMENT_ELEMENT.dispatchEvent(SCROLL_EVENT);
            }, 5);
        },
        subscribeScrollEvent: function () {
            if (!this._isActive) {
                const option = !document.documentMode ? { passive: true } : false;

                this._isActive = true;
                document.addEventListener('scroll', this._scrollEvent, option);
            }
        }
    };
    Object.defineProperties(ScrollController.prototype, {
        scrollPositionTop: {
            get: function () {
                return this._scrollPositionTop;
            },
            set: function () {
                throw new Error('this is not allowed operation');
            }
        }
    });

    /** @type {ScrollController} */
    const scrollController = new ScrollController();

    function FocusController() {
        this.init();
    }
    FocusController.prototype = {
        init: function () {
            /** @type {NodeList} */
            this._focusableElm = document.querySelectorAll(FOCUSABLE);
        },
        /**
         * Set the tab index of the passed Node to -1
         * @param {Node} elm
         */
        disable: function (elm) {
            elm.setAttribute('tabindex', '-1');
            elm.classList.add(DISABLE_TAB_INDEX_CLASSNAME);
        },
        /**
         * Disable the tab index for the entire page
         * @param {string} [parentClassName] Elements enclosed in this class are exceptions
         */
        disableTabIndex: function (parentClassName) {
            // Rescan the DOM to include dynamically added buttons, etc.
            this.init();
            for (let i = 0, len = this._focusableElm.length; i < len; i++) {
                const elm = this._focusableElm.item(i);

                if (parentClassName) {
                    if (!elm.closest('.' + parentClassName)) {
                        this.disable(elm);
                    }
                } else {
                    this.disable(elm);
                }
            }
        },
        /**
         * Reset the disabled tab index
         */
        resetTabIndex: function () {
            const disabled = document.querySelectorAll('.' + DISABLE_TAB_INDEX_CLASSNAME);

            for (let i = 0, len = disabled.length; i < len; i++) {
                disabled.item(i).setAttribute('tabindex', '');
            }
        }
    };

    const focusController = new FocusController();

    // resize event
    (function () {
        let timer = 0;

        // Fire CustomEvent after waiting 100ms
        function onResize() {
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }

            timer = setTimeout(function () {
                DOCUMENT_ELEMENT.dispatchEvent(RESIZE_EVENT);
            }, 100);
        }

        window.addEventListener('resize', onResize);
    }());

    function CustomFileLoader() {
        this._callback = [];
    }

    CustomFileLoader.prototype = {
        /**
         * Read XML asynchronously (flyout)
         * @param {string} url
         * @param {Function} callback
         */
        loadHTML: function (url, callback) {
            const xhr = new XMLHttpRequest();

            xhr.id = url;

            this._callback[url] = callback;

            xhr.addEventListener('readystatechange', this.onReadyStateChangeForHTML.bind(this));
            xhr.overrideMimeType('text/html');
            xhr.open('GET', url);
            xhr.send();
        },
        /**
         * Read XML asynchronously (generic filter)
         * @param url
         * @param callback
         */
        loadXML: function (url, callback) {
            const xhr = new XMLHttpRequest();

            xhr.id = url;

            this._callback[url] = callback;

            xhr.addEventListener('readystatechange', this.onReadyStateChangeForXML.bind(this));
            xhr.overrideMimeType('text/xml');
            xhr.open('GET', url);
            xhr.send();
        },
        onReadyStateChangeForHTML: function (e) {
            const xhr = e.target;

            if (xhr.readyState === 4 && xhr.status === 200) {
                this._callback[xhr.id](xhr.response);
            }
        },
        onReadyStateChangeForXML: function (e) {
            const xhr = e.target;

            if (xhr.readyState === 4 && xhr.status === 200) {
                this._callback[xhr.id](xhr.responseXML);
            }
        }
    };

    const fileLoader = new CustomFileLoader();

    // Control the behavior of the feature panel on the TOP page
    (function () {
        const panelElements = document.querySelectorAll('.js-top-panel-item');
        const panelElementsLength = panelElements.length;
        const instances = [];
        const WINDOW_HEIGHT_HALF = window.innerHeight * 0.5;

        if (panelElementsLength < 1) {
            return;
        }

        /**
         * Class that controls the fade-in/out behavior of the panel
         * @param _root
         * @constructor
         */
        function FadeImage(_root) {
            this._currentIndex = 0;
            /** @type {NodeListOf<HTMLUListElement>} */
            this._listItems = _root.querySelectorAll('.js-fade-image-item');
            this._listItemsLength = this._listItems.length;
            this._isEnabled = this._listItems.length > 2;
            this._timer = 0;
            this._intervalMillSecond = 4.5;
            this._listItems.item(this._currentIndex).classList.add('-show');
        }
        FadeImage.prototype = {
            play: function () {
                if (!this._isEnabled) {
                    return;
                }

                this.loop(60 * (this._intervalMillSecond - 2.5));
            },
            showNextImage: function () {
                this._listItems.item(this._currentIndex).classList.remove('-show');
                this._currentIndex++;
                this._currentIndex = this._listItemsLength === this._currentIndex ? 0 : this._currentIndex;
                this._listItems.item(this._currentIndex).classList.add('-show');
            },
            loop: function (_count) {
                let count = _count;

                if (count-- < 0) {
                    count = 60 * this._intervalMillSecond;
                    this.showNextImage();
                }
                this._timer = requestAnimationFrame(this.loop.bind(this, count));
            },
            pause: function () {
                cancelAnimationFrame(this._timer);
            }
        };

        /**
         * Define the scrolling behavior of each panel
         * @param {HTMLElement} _root
         * @param {boolean} _isFirst
         * @param {boolean} _isLast
         * @constructor
         */
        function TopPanel(_root, _isFirst, _isLast) {
            const fadeImage = _root.querySelector('.js-fade-image-root');

            this._root = _root;
            this._isFirst = _isFirst;
            this._isLast = _isLast;
            this._currentState = false;
            this.setThreshold();

            if (fadeImage) {
                this._fadeImage = new FadeImage(fadeImage);
            }
        }
        TopPanel.prototype = {
            setThreshold: function () {
                const position = this._root.getBoundingClientRect();

                this._thresholdTop = scrollController.scrollPositionTop + position.top;
                this._thresholdBottom = this._thresholdTop + this._root.offsetHeight;
            },
            activate: function () {
                this._currentState = true;
                this._root.classList.remove('-ready');
                this._root.classList.add('-current');
                this._fadeImage.play();
            },
            inactivate: function () {
                this._currentState = false;
                this._root.classList.remove('-current');
                this._fadeImage.pause();
            },
            judge: function () {
                const center = WINDOW_HEIGHT_HALF + scrollController.scrollPositionTop;

                if (!this._currentState) {
                    // The first part is activated if the bottom edge is below the screen center
                    if (this._isFirst) {
                        if (center < this._thresholdBottom) {
                            this.activate();
                        }
                        // The last part is activated if the top edge is above the screen center
                    } else if (this._isLast) {
                        if (center > this._thresholdTop) {
                            this.activate();
                        }
                        // Activated if the top edge is above the screen center and the bottom edge is below the screen center
                    } else if (center > this._thresholdTop && center < this._thresholdBottom) {
                        this.activate();
                    }
                } else {
                    // The first part is inactive if the bottom edge is above the screen center
                    if (this._isFirst) {
                        if (center > this._thresholdBottom) {
                            this.inactivate();
                        }
                        // The last part is inactive if the top edge is below the screen center
                    } else if (this._isLast) {
                        if (center < this._thresholdTop) {
                            this.inactivate();
                        }
                        // Inactive when the top edge is below the screen center or when the bottom edge is above the screen center
                    } else if (center < this._thresholdTop || center > this._thresholdBottom) {
                        this.inactivate();
                    }
                }
            }
        };

        function onScroll() {
            for (let i = 0; i < panelElementsLength; i++) {
                instances[i].judge();
            }
        }

        for (let i = 0; i < panelElementsLength; i++) {
            instances.push(new TopPanel(panelElements.item(i), i === 0, i === panelElementsLength - 1));
        }

        scrollController.subscribeScrollEvent();
        DOCUMENT_ELEMENT.addEventListener(SCROLL_EVENT_NAME, onScroll);
        onScroll();
    }());

    // Disclosure (toggle)
    (function () {
        const disclosureElement = document.querySelectorAll('.js-disclosure-root');
        const DISCLOSURE_EVENT = {
            TOGGLE: 'disclosureToggleEvent'
        };
        const DISCLOSURE_MODE = {
            SP_ONLY: 'sp-only',
            BOTH: 'both'
        };
        const DISCLOSURE_TYPE = {
            NORMAL: 'normal',
            ACCORDION: 'accordion'
        };
        const disclosureEvent = new CustomEvent(DISCLOSURE_EVENT.TOGGLE);
        const disclosureGroupArr = [];

        /**
         * @param {HTMLElement} _item
         * @param {string} _mode Mode (whether to make function for both PC and SP or only when SP is used)
         * @param {string} _index Index number used for id attribute
         * @constructor
         */
        function Disclosure(_item, _mode, _index) {
            this._mode = _mode;
            this._item = _item;
            this._content = _item.querySelector('.js-disclosure-contents');
            this._trigger = _item.querySelector('.js-disclosure-trigger');
            this._id = 'disclosure-contents-' + _index;
            this._isEnabled = false;

            if (this._trigger && this._content) {
                this._trigger.addEventListener('click', this.onClick.bind(this));
                this._content.addEventListener('transitionend', this.onTransitionEnd.bind(this));
                this.setTriggerText();
            } else {
                this._trigger.classList.add('-no-disclosure-body');
            }
        }

        Disclosure.prototype = {
            init: function () {
                this._content.setAttribute('id', this._id);
                this._trigger.setAttribute('aria-controls', this._id);
                this._trigger.setAttribute('aria-expanded', 'false');

                if (!this._isOpen) {
                    this.close();
                }
            },
            /**
             * Set the button label dynamically
             */
            setTriggerText: function () {
                const textOpen = this._trigger.querySelector('.js-disclosure-text-open');
                const textClose = this._trigger.querySelector('.js-disclosure-text-close');

                if (textOpen) {
                    textOpen.textContent = window.localData.text.more || 'See more';
                }

                if (textClose) {
                    textClose.textContent = window.localData.text.close || 'Close';
                }
            },
            /**
             * Check if the toggle feature is enabled
             * @returns {boolean}
             */
            checkEnabled: function () {
                if (this._trigger && this._content) {
                    if (this._mode === DISCLOSURE_MODE.BOTH) {
                        return true;
                    } else if (this._mode === DISCLOSURE_MODE.SP_ONLY && matchMedia.matches) {
                        return true;
                    }
                }

                return false;
            },
            onClick: function (e) {
                if (!this._isEnabled) {
                    return;
                }

                e.preventDefault();

                if (this._isOpen) {
                    this.close();
                } else {
                    this._item.dispatchEvent(disclosureEvent);
                    this.open();
                }
            },
            onTransitionEnd: function () {
                // Remove the fixed value to match the height during resizing to the content
                if (this._isOpen) {
                    this._content.style.height = '';
                }
            },
            open: function () {
                const self = this;

                // Get the height just before opening
                this._content.style.height = '';

                const height = this._content.offsetHeight;

                // Specify 0 to make a transition
                this._content.style.height = '0';
                setTimeout(function () {
                    self._content.classList.add('-open');
                    self._trigger.classList.add('-open');
                    self._content.style.height = height + 'px';
                    self._trigger.setAttribute('aria-expanded', 'true');
                    self._isOpen = true;
                }, 10);
            },
            resetOpenState: function () {
                this._content.classList.remove('-open');
                this._trigger.classList.remove('-open');
                this._isOpen = false;
            },
            close: function () {
                const self = this;

                // Respecify the height to make a transition
                this._content.style.height = this._content.offsetHeight + 'px';
                setTimeout(function () {
                    self._content.style.height = '0';
                    self.resetOpenState();
                    self._trigger.setAttribute('aria-expanded', 'false');
                }, 10);
            },
            reset: function () {
                this._content.style.height = '';
                this.resetOpenState();
                this._trigger.removeAttribute('aria-expanded');
                this._trigger.removeAttribute('aria-controls');
            },
            /**
             * Change the state when the media match status changes
             */
            setDisclosureMode: function () {
                const self = this;

                this._isEnabled = this.checkEnabled();

                if (this._trigger && this._content) {
                    // Add an attribute at the timing when the opening/closing function is switched effectively
                    if (this._isEnabled) {
                        this.init();
                    } else {
                        setTimeout(function () {
                            self.reset();
                        }, 0);
                    }
                }
            }
        };

        /**
         * For the accordion type, create a group class because you must close one before opening the other
         * @param {HTMLElement} _root
         * @param {number} _index
         * @constructor
         */
        function DisclosureGroup(_root, _index) {
            this._mode = _root.dataset.disclosureMode || DISCLOSURE_MODE.BOTH;
            this._type = _root.dataset.toggleType || DISCLOSURE_TYPE.NORMAL;
            /** @type {Disclosure[]} */
            this._instances = [];
            this._itemElm = _root.querySelectorAll('.js-disclosure-item');

            for (let i = 0, len = this._itemElm.length; i < len; i++) {
                const elm = this._itemElm.item(i);
                const instance = new Disclosure(elm, this._mode, _index + '-' + i);

                this._instances.push(instance);
                elm.addEventListener(DISCLOSURE_EVENT.TOGGLE, this.toggleItems.bind(this));
            }
        }
        DisclosureGroup.prototype = {
            /**
             * Determine whether to make toggle function
             */
            setDisclosureMode: function () {
                for (let i = 0, len = this._instances.length; i < len; i++) {
                    this._instances[i].setDisclosureMode();
                }
            },
            /**
             * If the toggle type is accordion, close other items
             */
            toggleItems: function () {
                if (this._type === DISCLOSURE_TYPE.ACCORDION) {
                    for (let i = 0, len = this._instances.length; i < len; i++) {
                        this._instances[i].close();
                    }
                }
            }
        };

        function onChangeMatchMedia() {
            for (let i = 0, len = disclosureGroupArr.length; i < len; i++) {
                disclosureGroupArr[i].setDisclosureMode();
            }
        }

        for (let i = 0, len = disclosureElement.length; i < len; i++) {
            disclosureGroupArr.push(new DisclosureGroup(disclosureElement.item(i), i));
        }

        matchMedia.addListener(onChangeMatchMedia);
        onChangeMatchMedia();
    }());

    // Alt text display for images
    (function () {
        const imageAltElement = document.querySelectorAll('.js-rwd-image-alt');
        const ADD_CLASS_NAME = 'js-alt-description';
        const instances = [];

        /**
         * Function to display alt text of image on screen only when SP is used
         * @param {HTMLElement} _root
         * @constructor
         */
        function RwdImageAlt(_root) {
            this._root = _root;
            this._img = _root.querySelector('img');
            this._altText = this._img.getAttribute('alt');
            this._altTextAlign = _root.dataset.jsTextAlign || '';
            this._isAdded = false;
        }

        RwdImageAlt.prototype = {
            /**
             * Add alt text as HTML element
             */
            addText: function () {
                if (!this._isAdded) {
                    this._root.insertAdjacentHTML('beforeend', '<p class="m-caption ' + ADD_CLASS_NAME + ' ' + this._altTextAlign + '">' + this._altText.replace('\r', '').replace('\n', '<br>') + '</p>');
                    this._img.setAttribute('alt', '');
                    this._isAdded = true;
                }
            },
            /**
             * Delete HTML element and store text in alt
             */
            removeText: function () {
                const captionElm = this._root.querySelector('.' + ADD_CLASS_NAME);

                if (captionElm) {
                    this._img.setAttribute('alt', this._altText);
                    this._root.removeChild(captionElm);
                }

                this._isAdded = false;
            },
            /**
             * Executed when the media match status changes
             */
            setDisplayMode: function () {
                if (matchMedia.matches) {
                    this.addText();
                } else {
                    this.removeText();
                }
            }
        };

        function onChangeMatchMedia() {
            for (let i = 0, len = instances.length; i < len; i++) {
                instances[i].setDisplayMode();
            }
        }

        for (let i = 0, len = imageAltElement.length; i < len; i++) {
            instances.push(new RwdImageAlt(imageAltElement.item(i)));
        }

        matchMedia.addListener(onChangeMatchMedia);
        onChangeMatchMedia();
    }());

    // Height adjustment
    (function () {
        const adjustHeightItem = document.querySelectorAll('.js-adjust-height');
        const adjustHeightInstance = [];

        function AdjustHeightItem(_root) {
            const data = _root.dataset.heightAdjust;

            this._root = _root;
            /** @type {NodeList[]} */
            this._adjustGroup = [];

            if (data) {
                const classNameList = data.split(',');

                for (let i = 0, len = classNameList.length; i < len; i++) {
                    this._adjustGroup.push(this._root.querySelectorAll(classNameList[i]));
                }
            }

            this._groupNum = this._adjustGroup.length;
        }

        AdjustHeightItem.prototype = {
            /**
             * Delete the currently set style
             */
            clearHeight: function () {
                for (let i = 0, len = this._groupNum; i < len; i++) {
                    const adjustNodeList = this._adjustGroup[i];

                    for (let j = 0, len2 = adjustNodeList.length; j < len2; j++) {
                        adjustNodeList.item(j).style.height = '';
                    }
                }
            },
            resetHeight: function () {
                this.clearHeight();
                this.adjust();
            },
            /**
             * Return the maximum height in the passed NodeList
             * @param {NodeList} _nodeList
             * @returns {number}
             */
            getMaxHeight: function (_nodeList) {
                let maxHeight = 0;

                for (let i = 0, len = _nodeList.length; i < len; i++) {
                    const node = _nodeList.item(i);
                    const height = node.offsetHeight;

                    if (maxHeight < height) {
                        maxHeight = height;
                    }
                }

                return maxHeight;
            },
            /**
             * Set the height style to the passed NodeList
             * @param {NodeList} _nodeList
             */
            setHeight: function (_nodeList) {
                const maxHeight = this.getMaxHeight(_nodeList) + 'px';

                for (let i = 0, len = _nodeList.length; i < len; i++) {
                    const node = _nodeList.item(i);

                    node.style.height = maxHeight;
                }
            },
            adjust: function () {
                for (let i = 0, len = this._groupNum; i < len; i++) {
                    this.setHeight(this._adjustGroup[i]);
                }
            }
        };

        function onResize() {
            for (let i = 0, len = adjustHeightInstance.length; i < len; i++) {
                adjustHeightInstance[i].resetHeight();
            }
        }

        for (let i = 0, len = adjustHeightItem.length; i < len; i++) {
            adjustHeightInstance.push(new AdjustHeightItem(adjustHeightItem.item(i)));
        }

        DOCUMENT_ELEMENT.addEventListener(RESIZE_EVENT_NAME, onResize);
        onResize();
    }());

    // Popup
    (function () {
        // const closeText = window.localData.text.close || 'Close';
        const closeText = 'Close';
        const CONTENTS_ROOT_CLASSNAME = 'js-modal-contents-root';
        const modalItems = document.querySelectorAll('.js-modal-root');
        const TEMPLATE = '<div class="m-modal ' + CONTENTS_ROOT_CLASSNAME + ' -hide">' +
            '<div class="m-modal__overlay js-modal-close"></div>' +
            '<div class="m-modal__container">' +
            '<div class="m-modal__body">{{body}}</div>' +
            '<button class="m-modal__close js-modal-close" type="button"><span class="m-modal__close__text">' + closeText + '</span></button>' +
            '</div>' +
            '</div>';
        const MODAL_TYPE = {
            INLINE: 'inline',
            IFRAME: 'iframe',
            VIDEO: 'video'
        };
        /** @type {Modal} */
        let current;

        /**
         * Close the currently open modal when you press the Esc key
         * @param {KeyboardEvent} e
         */
        function onModalKeydownListener(e) {
            if (e.key === 'Esc' || e.key === 'Escape') {
                if (current) {
                    current.hide();
                }
            }
        }

        function Modal(_root, _index) {
            const type = _root.dataset.modalType;

            this._root = _root;
            this._type = type ? type : MODAL_TYPE.INLINE;
            this._trigger = _root.querySelector('.js-modal-open');
            this._trigger.setAttribute('id', 'js-modal-trigger-' + _index);
            this._contentRoot = null;
            const target = this._trigger ? this._trigger.getAttribute('href') : '';

            if (this._type !== MODAL_TYPE.INLINE && this._type !== MODAL_TYPE.IFRAME && this._type !== MODAL_TYPE.VIDEO) {
                throw new Error('Specify "inline", "iframe" or "video" for data-modal-type attribute。');
            }

            this._target = document.getElementById(target.replace('#', ''));
            this._target.parentElement.removeChild(this._target); // Delete the original content

            if (this._target) {
                this.init();
            }
        }

        Modal.prototype = {
            init: function () {
                this._trigger.addEventListener('click', this.generate.bind(this));
            },
            /**
             * Setting of an event to close dynamically inserted modal content (close when close button and overlay are clicked)
             */
            setCloseEvent: function () {
                const closeTrigger = this._contentRoot.querySelectorAll('.js-modal-close');

                for (let i = 0, len = closeTrigger.length; i < len; i++) {
                    const trigger = closeTrigger.item(i);

                    trigger.addEventListener('click', this.hide.bind(this));
                }
            },
            setModalAriaState: function (contents) {
                if (contents) {
                    contents.setAttribute('role', 'dialog');
                    contents.setAttribute('aria-labelledby', this._trigger.id);
                    contents.setAttribute('aria-modal', 'true');
                }
            },
            /**
             * After opening the modal, focus on the first element in the modal
             * @param {HTMLElement} firstChild
             */
            focusOnFirstChild: function (firstChild) {
                if (firstChild) {
                    setTimeout(function () {
                        firstChild.setAttribute('tabindex', '1');
                        firstChild.focus();
                    }, 100);
                }
            },
            /**
             * After closing the modal, return focus to the trigger
             */
            focusOnTrigger: function () {
                const self = this;

                setTimeout(function () {
                    self._trigger.focus();
                }, 100);
            },
            generate: function (e) {
                const self = this;
                let content = '';

                e.preventDefault();

                // Embed content into the template
                content = TEMPLATE.replace('{{body}}', this._target.outerHTML);

                // Close any items that are already open
                if (current) {
                    current.hide();
                }

                BODY.insertAdjacentHTML('beforeend', content);
                this._contentRoot = document.querySelector('.' + CONTENTS_ROOT_CLASSNAME);

                const contents = this._contentRoot.querySelector('.js-modal-contents');

                this.setCloseEvent();

                if (contents) {
                    this.setModalAriaState(contents);
                    this.focusOnFirstChild(contents.firstElementChild);
                }

                // Delay to make a transition
                setTimeout(function () {
                    self.show();
                }, 0);
            },
            show: function () {
                current = this;
                focusController.disableTabIndex(CONTENTS_ROOT_CLASSNAME);
                scrollController.lock();
                this._contentRoot.classList.remove('-hide');
                this._contentRoot.classList.add('-' + this._type);
                document.addEventListener('keyup', onModalKeydownListener);
            },
            onTransitionEnd: function () {
                if (current) {
                    current = null;
                    focusController.resetTabIndex();
                    scrollController.unlock();
                    BODY.removeChild(this._contentRoot);
                    this._contentRoot.classList.remove('-iframe');
                    this.focusOnTrigger();
                }
            },
            hide: function () {
                this._contentRoot.addEventListener('transitionend', this.onTransitionEnd.bind(this));
                this._contentRoot.classList.add('-hide');
                document.removeEventListener('keyup', onModalKeydownListener);
            }
        };

        for (let i = 0, len = modalItems.length; i < len; i++) {
            new Modal(modalItems.item(i), i); // eslint-disable-line
        }
    }());

    // Clickable map
    (function () {
        const clickableMapItem = document.querySelectorAll('img[usemap]');
        const instances = [];

        /**
         * Function to make clickable map responsive
         * @param {HTMLImageElement} _img
         * @constructor
         */
        function RwdImageMaps(_img) {
            const targetId = _img.getAttribute('usemap').replace('#', '');

            this._image = _img;
            this._target = document.querySelector('map[name="' + targetId + '"]');

            if (!this._target) {
                return;
            }

            /** @type {NodeListOf<HTMLAreaElement>} */
            this._area = this._target.querySelectorAll('area');
            this._areaNum = this._area.length;

            if (this._areaNum === 0) {
                return;
            }

            const image = new Image();

            // Wait for the image loading
            image.addEventListener('load', this.init.bind(this));
            image.src = this._image.src;
        }
        RwdImageMaps.prototype = {
            /**
             * Get the original size of the image and the coords attribute of area
             */
            init: function () {
                this._ordinalWidth = this._image.naturalWidth;
                this._ordinalHeight = this._image.naturalHeight;
                this._coords = [];

                for (let i = 0; i < this._areaNum; i++) {
                    this._coords.push(this._area[i].coords);
                }

                this.setPosition();
            },
            /**
             * Get the ratio of the current image width/height to original width/height
             */
            setRatio: function () {
                this._parcentWidth = this._image.offsetWidth / this._ordinalWidth;
                this._parcentHeight = this._image.offsetHeight / this._ordinalHeight;
            },
            /**
             * Recalculate the coords numbers according to the image ratio
             */
            setPosition: function () {
                this.setRatio();
                for (let i = 0; i < this._areaNum; i++) {
                    const area = this._area.item(i);
                    const coords = this._coords[i].split(',');
                    const newCoords = [];

                    for (let j = 0, coordsNum = coords.length; j < coordsNum; j++) {
                        if (j % 2 === 0) {
                            newCoords[j] = parseInt(coords[j], 10) * this._parcentWidth;
                        } else {
                            newCoords[j] = parseInt(coords[j], 10) * this._parcentHeight;
                        }
                    }

                    area.coords = newCoords.join(',');
                }
            }
        };

        function onResize() {
            for (let i = 0, len = instances.length; i < len; i++) {
                instances[i].setPosition();
            }
        }

        for (let i = 0, len = clickableMapItem.length; i < len; i++) {
            instances.push(new RwdImageMaps(clickableMapItem.item(i)));
        }

        DOCUMENT_ELEMENT.addEventListener(RESIZE_EVENT_NAME, onResize);
    }());

    // Sortable table
    (function () {
        const sortableTableElement = document.querySelectorAll('.js-table-sort');

        // Comparison function used for sorting
        const SORT_TYPE = {
            number: function (a, b) {
                return a.data - b.data;
            },
            string: function (_a, _b) {
                const a = String(_a.data);
                const b = String(_b.data);

                let result = 0;

                if (a < b) {
                    result = -1;
                } else if (a > b) {
                    result = 1;
                }

                return result;
            }
        };

        function SortableTable(_root) {
            this._root = _root;
            this._thead = _root.querySelector('thead');
            this._theadTh = this._thead.querySelectorAll('th');
            this._theadThArr = [].slice.call(this._theadTh);
            this._tbody = _root.querySelector('tbody');
            this._tbodyTr = this._tbody.querySelectorAll('tr');
            this._oridinalHTML = this._tbody.innerHTML;
            this.init();
        }

        SortableTable.prototype = {
            init: function () {
                // Enclose cells in thead with "a" tags
                for (let i = 0, len = this._theadTh.length; i < len; i++) {
                    const target = this._theadTh.item(i);

                    if (target.classList.contains('sort-denial')) {
                        continue;
                    }

                    target.innerHTML = '<button class="m-table__cell__button -default" type="button">' + target.innerHTML + '</button>';

                    const button = target.querySelector('button');

                    button.addEventListener('click', this.onClick.bind(this));
                }
            },
            onClick: function (e) {
                e.preventDefault();

                this.sort(e.currentTarget);
            },
            /**
             * Reset the classes attached to all thead triggers
             */
            resetStateClass: function () {
                const anchor = this._root.querySelectorAll('.-asc, .-desc');

                for (let i = 0, len = anchor.length; i < len; i++) {
                    anchor.item(i).setAttribute('class', 'm-table__cell__button -default');
                }
            },
            /**
             * Reset to the default order
             */
            changeToDefault: function () {
                this.resetStateClass();
                this._tbody.innerHTML = this._oridinalHTML;
                // Re-obtain the tr element after DOM construction
                this._tbodyTr = this._tbody.querySelectorAll('tr');
            },
            /**
             * Sort in ascending order
             * @param {boolean} isNumberSort
             * @param {HTMLAnchorElement} trigger
             * @param {{number: [], string: [], merge: []}} sortObj
             * @returns {{number: [], string: [], merge: []}}
             */
            changeToAsc: function (isNumberSort, trigger, sortObj) {
                this.resetStateClass();
                trigger.setAttribute('class', 'm-table__cell__button -asc');

                if (isNumberSort) {
                    sortObj.merge = sortObj.number.concat(sortObj.string);
                } else {
                    sortObj.merge.sort(SORT_TYPE.string);
                }

                return sortObj;
            },
            /**
             * Sort in descending order
             * @param {boolean} isNumberSort
             * @param {HTMLAnchorElement} trigger
             * @param {{number: [], string: [], merge: []}} sortObj
             * @returns {{number: [], string: [], merge: []}}
             */
            changeToDesc: function (isNumberSort, trigger, sortObj) {
                this.resetStateClass();

                trigger.setAttribute('class', 'm-table__cell__button -desc');

                if (isNumberSort) {
                    sortObj.number.reverse();
                    sortObj.string.reverse();
                    sortObj.merge = sortObj.string.concat(sortObj.number);
                } else {
                    sortObj.merge.sort(SORT_TYPE.string).reverse();
                }

                return sortObj;
            },
            /**
             * @param {HTMLAnchorElement} trigger
             */
            sort: function (trigger) {
                const parentCell = trigger.closest('th, td');
                const targetNumber = this._theadThArr.indexOf(parentCell);
                const isNumberSort = parentCell.classList.contains('sort-fix-number');
                let sortObj = {
                    number: [],
                    string: [],
                    merge: []
                };

                for (let i = 0, len = this._tbodyTr.length; i < len; i++) {
                    const targetCell = this._tbodyTr.item(i).querySelectorAll('th, td');
                    let targetRowData = targetCell.item(targetNumber).textContent;

                    if (targetRowData) {
                        targetRowData = targetRowData.trim();
                    }

                    if (/^[0-9.-]+$/.test(targetRowData)) {
                        sortObj.number.push({
                            idx: i,
                            data: isNumberSort ? targetRowData.match(/^[0-9.-]+$/).join('') : targetRowData
                        });
                    } else {
                        sortObj.string.push({
                            idx: i,
                            data: isNumberSort ? '' : targetRowData
                        });
                    }
                }

                sortObj.merge = sortObj.number.concat(sortObj.string);
                sortObj.number.sort(SORT_TYPE.number);
                sortObj.string.sort(SORT_TYPE.string);

                if (trigger.classList.contains('-default')) {
                    sortObj = this.changeToAsc(isNumberSort, trigger, sortObj);
                } else if (trigger.classList.contains('-asc')) {
                    sortObj = this.changeToDesc(isNumberSort, trigger, sortObj);
                } else if (trigger.classList.contains('-desc')) {
                    // Since the default comes after descending order, it returns to the state when the page was loaded
                    this.changeToDefault();

                    return;
                }

                let result = '';

                for (let i = 0, anl = sortObj.merge.length; i < anl; i++) {
                    const tr = this._tbodyTr.item(sortObj.merge[i].idx);

                    result += tr.outerHTML;
                }

                this._tbody.innerHTML = result;
                // Re-obtain the tr element after DOM construction
                this._tbodyTr = this._tbody.querySelectorAll('tr');
            }
        };

        for (let i = 0, len = sortableTableElement.length; i < len; i++) {
            new SortableTable(sortableTableElement.item(i)); // eslint-disable-line
        }
    }());

    // Tab
    (function () {
        const tabElement = document.querySelectorAll('.js-tab-root');
        const instances = [];

        function Tab(_root) {
            this._navigationItemList = _root.querySelectorAll('.js-tab-item');
            this._triggerList = _root.querySelectorAll('.js-tab-trigger');
            this._contentList = _root.querySelectorAll('.js-tab-contents-item');
            this._currentIndex = 0;
            this._initFlg = false;

            if (this._triggerList.length > 0 && this._contentList.length > 0) {
                this._maxIndex = this._triggerList.length;
                this.init();
            }
        }

        Tab.prototype = {
            init: function () {
                this._navigationItemList.item(0).parentElement.setAttribute('role', 'tablist');
                for (let i = 0, len = this._triggerList.length; i < len; i++) {
                    const anchor = this._triggerList.item(i);
                    const href = anchor.getAttribute('href') || '';
                    const contents = document.getElementById(href.replace('#', ''));

                    if (href && contents) {
                        contents.setAttribute('role', 'tabpanel');
                        contents.setAttribute('aria-labelledby', anchor.id);
                        anchor.setAttribute('aria-controls', contents.id);
                        anchor.setAttribute('role', 'tab');
                        anchor.addEventListener('click', this.onClickTrigger.bind(this));
                        anchor.addEventListener('keydown', this.onKeydownTrigger.bind(this));
                    }
                }

                const href = this._triggerList.item(this._currentIndex).getAttribute('href') || '';

                this.setState(href);
            },
            /**
             * Cancel current
             */
            clearState: function () {
                for (let i = 0, len = this._navigationItemList.length; i < len; i++) {
                    const item = this._navigationItemList.item(i);

                    item.classList.remove('-current');
                }
            },
            /**
             * Change the selection status of the tab part
             * @param {string} _href
             */
            setTriggerState: function (_href) {
                for (let i = 0, len = this._triggerList.length; i < len; i++) {
                    const anchor = this._triggerList.item(i);
                    const href = anchor.getAttribute('href') || '';

                    // Prevent focus from being applied to other tabs with the tab key
                    anchor.tabIndex = -1;

                    if (_href === href) {
                        const currentList = this._navigationItemList.item(i);

                        this._currentIndex = i;

                        if (currentList) {
                            currentList.classList.add('-current');
                        }

                        anchor.tabIndex = 0;

                        // Do not focus on the tab on the first run on page load
                        if (!this._initFlg) {
                            anchor.focus();
                        } else {
                            this._initFlg = false;
                        }

                        anchor.setAttribute('aria-selected', 'true');
                    } else {
                        anchor.setAttribute('aria-selected', 'false');
                    }
                }
            },
            /**
             * Set the state of the content part
             * @param {string} _href
             */
            setContentState: function (_href) {
                const id = _href.replace('#', '');

                for (let i = 0, len = this._contentList.length; i < len; i++) {
                    const contents = this._contentList.item(i);

                    if (contents.id === id) {
                        contents.hidden = false;
                        contents.tabIndex = 0;
                        continue;
                    }

                    contents.tabIndex = -1;
                    contents.hidden = true;
                }
            },
            setState: function (href) {
                if (!href) {
                    return;
                }

                this.clearState();
                this.setTriggerState(href);
                this.setContentState(href);
            },
            onClickTrigger: function (e) {
                e.preventDefault();

                const anchor = e.currentTarget;
                const href = anchor.getAttribute('href') || '';

                this.setState(href);
            },
            onKeydownTrigger: function (e) {
                const keyList = ['ArrowRight', 'Right', 'ArrowLeft', 'Left'];
                const key = e.key;
                const index = keyList.indexOf(key);

                if (index !== -1) {
                    e.preventDefault();
                    if (index < 2 && this._currentIndex < this._maxIndex - 1) {
                        this._currentIndex++;
                    } else if (index > 1 && this._currentIndex > 0) {
                        this._currentIndex--;
                    }

                    const href = this._triggerList.item(this._currentIndex).getAttribute('href') || '';

                    this.setState(href);
                }
            }
        };

        for (let i = 0, len = tabElement.length; i < len; i++) {
            instances.push(new Tab(tabElement.item(i)));
        }
    }());

    // Initialize YouTube video player
    (function (_roots) {
        if (_roots.length === 0) {
            return;
        }

        const instances = [];

        function YoutubeComponent(_root, index) {
            this._root = _root;
            this._iframe = _root.querySelector('.js-iframe-element');
            this._videoId = this._iframe.dataset.videoId;
            this._title = this._iframe.dataset.title || 'Youtube Player';
            this._allow = this._iframe.dataset.allow || '';
            this._id = 'js-iframe-element-' + index;
            this._iframe.id = this._id;
        }

        YoutubeComponent.prototype = {
            setIframeAttribute: function (element) {
                element.setAttribute('title', this._title);
                element.setAttribute('allow', this._allow);
            },

            removeIframeAttribute: function (element) {
                element.removeAttribute('allowfullscreen');
                element.removeAttribute('frameborder');
                element.removeAttribute('data-title');
                element.removeAttribute('data-allow');
                element.removeAttribute('data-video-id');
            },

            initIframeAttribute: function () {
                const iframeElement = this._root.querySelector('#' + this._id);

                this.removeIframeAttribute(iframeElement);
                this.setIframeAttribute(iframeElement);
            },

            setIframe: function () {
                const ytPlayer = new YT.Player( // eslint-disable-line
                    this._id,
                    {
                        videoId: this._videoId,
                        width: '',
                        height: ''
                    }
                );

                this.initIframeAttribute();
            }
        };

        function setIframePlayerApi() {
            const tag = document.createElement('script');
            const firstScriptTag = document.querySelectorAll('script').item(0);

            tag.src = 'https://www.youtube.com/iframe_api';
            tag.defer = true;

            firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
        }

        window.onYouTubeIframeAPIReady = function () {
            for (let i = 0, len = _roots.length; i < len; i++) {
                instances.push(new YoutubeComponent(_roots.item(i), i));
                instances[i].setIframe();
            }
        };

        setIframePlayerApi();
    }(document.querySelectorAll('.js-youtube-iframe')));

    // Change the appearance according to the input status of the form
    (function () {
        function setInputFormItem(formItems) {
            if (formItems.length === 0) {
                return;
            }

            const instances = [];

            function InputFormItem(_item) {
                this._item = _item;

                _item.addEventListener('keydown', this.checkInput.bind(this));
                _item.addEventListener('change', this.checkInput.bind(this));
            }

            InputFormItem.prototype = {
                inputed: function () {
                    this._item.classList.add('-inputed');
                },

                notInputed: function () {
                    this._item.classList.remove('-inputed');
                },

                checkInput: function (e) {
                    return e.target.value === '' ? this.notInputed() : this.inputed();
                }
            };

            for (let i = 0, len = formItems.length; i < len; i++) {
                instances.push(new InputFormItem(formItems.item(i)));
            }
        }

        customModules.setInputFormItem = setInputFormItem;
        setInputFormItem(document.querySelectorAll('.js-form-item'));
    }());

    // Google Maps
    (function () {
        /* global google */
        const gmapElement = document.querySelectorAll('.js-gmap');

        if (gmapElement.length === 0) {
            return;
        }

        function setGoogleMaps(root) {
            const center = {
                lat: Number.parseFloat(root.dataset.jsLat),
                lng: Number.parseFloat(root.dataset.jsLng)
            };
            const label = root.dataset.jsText || '';
            const radius = Number.parseInt(root.dataset.jsRadius, 10) || 1;
            const map = new google.maps.Map(root, {
                center: center
            });
            const leftBottomBounds = new google.maps.LatLng(center.lat + (0.0090133729745762 * radius), center.lng);
            const rightTopBounds = new google.maps.LatLng(center.lat - (0.0090133729745762 * radius), center.lng);
            const latLngBounds = new google.maps.LatLngBounds(leftBottomBounds, rightTopBounds);

            map.fitBounds(latLngBounds, 0);

            const marker = new google.maps.Marker({
                position: center,
                map: map
            });

            if (label) {
                const infoWindow = new google.maps.InfoWindow({
                    content: '<div class="sample">' + label + '</div>'
                });

                infoWindow.open(map, marker);
            }
        }

        /**
         * Callback function executed when the script for Google Map is completely loaded
         */
        window.onGmapReady = function () {
            for (let i = 0, len = gmapElement.length; i < len; i++) {
                setGoogleMaps(gmapElement.item(i));
            }

            delete window.onGmapReady;
        };

        /**
         * Script loading for Google Maps API
         * @param {Element} root
         */
        function init(root) {
            const tag = document.createElement('script');
            const firstScriptTag = document.querySelectorAll('script').item(0);

            tag.src = 'https://maps.googleapis.com/maps/api/js?key=' + root.dataset.jsKey + '&callback=onGmapReady';
            tag.defer = true;

            firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
        }

        if (gmapElement.length > 0) {
            init(gmapElement.item(0));
        }
    }());

    // Smooth scroll
    (function (items) {
        if (items.length === 0) {
            return;
        }

        const instances = [];
        let timerId = null;

        function SmoothScroll(_item) {
            this._item = _item;
            this._href = _item.getAttribute('href');
            this._target = this._href === '#top' ? BODY : document.querySelector(this._href);
            this.scrollEvent = this.scrollEvent.bind(this);

            _item.addEventListener('click', this.smoothScroll.bind(this));
        }

        SmoothScroll.prototype = {
            getPosition: function () {
                return this._target.offsetTop;
            },
            setFocus: function () {
                this._target.setAttribute('tabindex', '0');
                this._target.focus();
                this._target.removeAttribute('tabindex');
                window.removeEventListener('scroll', this.scrollEvent);
            },
            scrollEvent: function () {
                const self = this;

                clearTimeout(timerId);
                timerId = setTimeout(function () {
                    self.setFocus();
                }, 100);
            },
            smoothScroll: function (e) {
                e.preventDefault();

                window.addEventListener('scroll', this.scrollEvent);
                window.scroll({
                    top: this.getPosition(),
                    behavior: 'smooth'
                });
            }
        };

        for (let i = 0, len = items.length; i < len; i++) {
            instances.push(new SmoothScroll(items.item(i)));
        }
    }(document.querySelectorAll('.js-smooth-scroll')));

    // Generic filter
    (function () {
        const drillDownElement = document.querySelector('.js-drill-down');

        if (!drillDownElement) {
            return;
        }

        function FilterModel() {
            this._strcutureData = null;
            this._filterData = null;
            this._currentSelectState1 = null;
            this._currentSelectData1 = null;
            this._currentSelectState2 = null;
            this._currentSelectData2 = null;
            this._currentSelectState3 = null;
            this._currentSelectData3 = null;
        }
        FilterModel.prototype = {
            /**
             * Get data to be inserted in the select box of STEP1
             * @returns {NodeList}
             */
            getStep1Data: function () {
                return this._filterData.querySelectorAll('any1Data');
            },
            /**
             * Get data to be inserted in the select box of STEP2
             * @returns {NodeList}
             */
            getStep2Data: function () {
                return this._currentSelectData1.querySelectorAll('any2Data');
            },
            /**
             * Get data to be inserted in the select box of STEP3
             * @returns {NodeList}
             */
            getStep3Data: function () {
                return this._currentSelectData2.querySelectorAll('any3Data');
            },
            getResult: function () {
                return this._currentSelectData3;
            },
            /**
             * Text to display in results area
             * @returns {string}
             */
            getRelatedTitleText: function () {
                return this._strcutureData.querySelector('anyTitle5').textContent;
            },
            /**
             * Get the flag for whether to deactivate step 2
             * @returns {boolean} Deactivate if true
             */
            getStep2DisabledState: function () {
                if (this._currentSelectData1 === null) {
                    return false;
                }

                const step2Data = this.getStep2Data();

                return this._currentSelectState1 !== null && (step2Data.length === 1 && step2Data.item(0).textContent === '');
            },
            /**
             * Get the flag for whether to deactivate step 3
             * @returns {boolean} Deactivate if true
             */
            getStep3DisabledState: function () {
                if (this._currentSelectData2 === null) {
                    return false;
                }

                const step3Data = this.getStep3Data();

                return step3Data.length === 1 && step3Data.item(0).textContent === '';
            }
        };
        Object.defineProperties(FilterModel.prototype, {
            structureData: {
                get: function () {
                    return this._strcutureData;
                },
                set: function (value) {
                    this._strcutureData = value;
                }
            },
            filterData: {
                get: function () {
                    return this._filterData;
                },
                set: function (value) {
                    this._filterData = value;
                }
            },
            currentSelectState1: {
                get: function () {
                    return this._currentSelectState1;
                },
                set: function (_value) {
                    const value = _value === '' ? null : _value;

                    this._currentSelectState1 = value;

                    if (value === null) {
                        // If step 1 becomes null, also reset the subsequent steps
                        this._currentSelectData1 = null;
                    } else {
                        this._currentSelectData1 = this._filterData.querySelectorAll('anyRoot').item(this._currentSelectState1);
                    }
                }
            },
            currentSelectState2: {
                get: function () {
                    return this._currentSelectState2;
                },
                set: function (_value) {
                    const value = _value === '' ? null : _value;

                    this._currentSelectState2 = value;

                    if (value === null) {
                        // If step 2 becomes null, also reset the subsequent steps
                        this._currentSelectData2 = null;
                    } else {
                        this._currentSelectData2 = this._currentSelectData1.querySelectorAll('group').item(this._currentSelectState2);
                    }
                }
            },
            currentSelectState3: {
                get: function () {
                    return this._currentSelectState3;
                },
                set: function (_value) {
                    const value = _value === '' ? null : _value;

                    this._currentSelectState3 = value;

                    if (value === null) {
                        this._currentSelectData3 = null;
                    } else {
                        this._currentSelectData3 = this._currentSelectData2.querySelectorAll('item').item(this._currentSelectState3);
                    }
                }
            }
        });
        const FILTER_HTML_TEMPLATE = '<div class="m-filter__list js-filter-root">' +
            '<div class="m-filter__condition">' +
            '<ol>' +
            '<li>' +
            '<div class="m-form m-form--select">' +
            '<fieldset>' +
            '<legend class="m-form__label"><label for="filter-select-1">{{anyTitle1}}</label></legend>' +
            '<div class="m-form-inner"><div class="m-form__select" data-order="0">' +
            '<select class="m-form__select-item js-form-item js-filter-select-1" id="filter-select-1"></select>' +
            '</div></div>' +
            '</fieldset>' +
            '</div>' +
            '</li>' +
            '<li>' +
            '<div class="m-form m-form--select">' +
            '<fieldset>' +
            '<legend class="m-form__label"><label for="filter-select-2">{{anyTitle2}}</label></legend>' +
            '<div class="m-form-inner"><div class="m-form__select" data-order="0">' +
            '<select class="m-form__select-item js-form-item js-filter-select-2" id="filter-select-2"></select>' +
            '</div></div>' +
            '</fieldset>' +
            '</div>' +
            '</li>' +
            '<li>' +
            '<div class="m-form m-form--select">' +
            '<fieldset>' +
            '<legend class="m-form__label"><label for="filter-select-3">{{anyTitle3}}</label></legend>' +
            '<div class="m-form-inner"><div class="m-form__select" data-order="0">' +
            '<select class="m-form__select-item js-form-item js-filter-select-3" id="filter-select-3"></select>' +
            '</div></div>' +
            '</fieldset>' +
            '</div>' +
            '</li>' +
            '</ol>' +
            '</div>' +
            '<div class="m-filter__result">' +
            '<p class="m-filter__result__title">{{anyTitle4}}</p>' +
            '<div class="m-filter__result__wrap">' +
            '<p class="m-filter__attention">{{anyTitle8}}</p>' +
            '<div class="m-filter__result__inner js-filter-result"></div>' +
            '</div>' +
            '</div>';
        const RESULT_HTML_TEMPLATE = '<div class="m-filter__result__info"><p class="m-filter__link">' +
            '<a href="{{exportUrl}}" class="btn btn-link"{{targetAttribute}}><span class="btn-link__inner">{{exportText}}{{exportIconImage}}</span></a>' +
            '</p>{{relatedText}}{{descriptionText}}{{tagList}}</div>{{thumbnailText}}';
        const RELATED_HTML_TEMPLATE = '<div class="m-filter__related">' +
            '<p class="m-filter__related__title">{{relatedTitleText}}</p>' +
            '<div class="m-link-list"><div class="m-link-list__body"><ul>{{relatedList}}</ul></div></div>' +
            '</div>';
        const TAG_HTML_TEMPLATE = '<div class="m-list-tag"><ul>{{tagListText}}</ul></div>';
        const MAX_XML_COUNT = 2;
        const filterModel = new FilterModel();
        let loadedXmlCount = 0;

        function setGeneralFilter(_root) {
            if (!_root) {
                return;
            }

            /** @type {HTMLSelectElement} */
            const select1 = _root.querySelector('.js-filter-select-1');
            /** @type {HTMLSelectElement} */
            const select2 = _root.querySelector('.js-filter-select-2');
            const selectWrap2 = select2.closest('.m-form--select');
            /** @type {HTMLSelectElement} */
            const select3 = _root.querySelector('.js-filter-select-3');
            const selectWrap3 = select3.closest('.m-form--select');
            const resultElm = _root.querySelector('.js-filter-result');
            const resultParent = resultElm.parentElement;

            /**
             * Create HTML of option element from the received data
             * @param {string} defaultText Text to set in the first option element
             * @param {NodeList} data
             */
            function generatePullDownHTMLText(defaultText, data) {
                let html = '<option value="">' + defaultText + '</option>';

                for (let i = 0, len = data.length; i < len; i++) {
                    html += '<option value="' + i + '">' + data.item(i).textContent + '</option>';
                }

                return html;
            }

            /**
             * Generate HTML text for related information displayed in the results area
             * @param {Node} result
             * @returns {string}
             */
            function generateRelatedListHTMLText(result) {
                const related1 = result.querySelector('related1');
                const relatedUrl1 = result.querySelector('relatedUrl1');
                const related2 = result.querySelector('related2');
                const relatedUrl2 = result.querySelector('relatedUrl2');
                const related3 = result.querySelector('related3');
                const relatedUrl3 = result.querySelector('relatedUrl3');
                let html = '';

                if (related1) {
                    html += '<li><a href="' + relatedUrl1.textContent + '" class="btn btn-link"><span class="btn-link__inner m-icon__arrow-right">' + related1.textContent + '</a></li>';
                }

                if (related2) {
                    html += '<li><a href="' + relatedUrl2.textContent + '" class="btn btn-link"><span class="btn-link__inner m-icon__arrow-right">' + related2.textContent + '</a></li>';
                }

                if (related3) {
                    html += '<li><a href="' + relatedUrl3.textContent + '" class="btn btn-link"><span class="btn-link__inner m-icon__arrow-right">' + related3.textContent + '</a></li>';
                }

                if (html !== '') {
                    html = RELATED_HTML_TEMPLATE.replace('{{relatedTitleText}}', filterModel.getRelatedTitleText()).replace('{{relatedList}}', html);
                }

                return html;
            }

            function generateThumbnailHTMLText(result) {
                const THUMBNAIL_HTML_TEMPLATE = '<div class="m-filter__link__image"><img src="{{thumbnailUrl}}" alt=""></div>';
                const thumbnail = result.querySelector('thumbnailUrl');

                if (thumbnail) {
                    return THUMBNAIL_HTML_TEMPLATE.replace('{{thumbnailUrl}}', thumbnail.textContent);
                }

                return '';
            }

            function generateDescriptionHTMLText(result) {
                const THUMBNAIL_HTML_TEMPLATE = '<p class="m-filter__result__description">{{descriptionText}}</p>';
                const description = result.querySelector('descriptionText');

                if (description) {
                    return THUMBNAIL_HTML_TEMPLATE.replace('{{descriptionText}}', description.textContent);
                }

                return '';
            }

            /**
             * Convert the tags described in XML to HTML text and return
             * @param result
             * @returns {string}
             */
            function generateTagListHTMLText(result) {
                const LIST_HTML_TEMPLATE = '<li class="m-label">{{text}}</li>';
                const tagList = result.querySelectorAll('tagList > tagListItem');
                let html = '';

                for (let i = 0, len = tagList.length; i < len; i++) {
                    const listItem = tagList.item(i);

                    html += LIST_HTML_TEMPLATE.replace('{{text}}', listItem.textContent);
                }

                if (html !== '') {
                    html = TAG_HTML_TEMPLATE.replace('{{tagListText}}', html);
                }

                return html;
            }

            /**
             * View filtered results
             */
            function generateResult() {
                /** @type {Node} */
                const result = filterModel.getResult();

                if (result === null) {
                    resultParent.classList.remove('-complete');

                    return;
                }

                resultParent.classList.add('-complete');

                const altText = window.localData.text.targetBlankIconAltText || 'Open in a new window';
                const exportText = result.querySelector('exportText').textContent;
                const exportUrl = result.querySelector('exportUrl').textContent;
                const exportIconFlg = result.querySelector('exportIconFlg');
                const exportIconFlgText = exportIconFlg ? exportIconFlg.textContent : '';
                const thumbnailText = generateThumbnailHTMLText(result);
                const relatedText = generateRelatedListHTMLText(result);
                const descriptionText = generateDescriptionHTMLText(result);
                const tagList = generateTagListHTMLText(result);
                let finalHTMLText = '';

                finalHTMLText = RESULT_HTML_TEMPLATE.replace('{{exportText}}', exportText);
                finalHTMLText = finalHTMLText.replace('{{exportUrl}}', exportUrl);
                finalHTMLText = finalHTMLText.replace('{{thumbnailText}}', thumbnailText);
                finalHTMLText = finalHTMLText.replace('{{descriptionText}}', descriptionText);
                finalHTMLText = finalHTMLText.replace('{{relatedText}}', relatedText);
                finalHTMLText = finalHTMLText.replace('{{tagList}}', tagList);

                if (exportIconFlgText === '1') {
                    finalHTMLText = finalHTMLText.replace('{{targetAttribute}}', ' target="_blank"');
                    finalHTMLText = finalHTMLText.replace('{{exportIconImage}}', '<img class="m-btn__blank-icon" src="/resources/eng/images/icn_window-g.svg" alt="' + altText + '">');
                } else {
                    finalHTMLText = finalHTMLText.replace('{{targetAttribute}}', '');
                    finalHTMLText = finalHTMLText.replace('{{exportIconImage}}', '');
                }

                resultElm.innerHTML = finalHTMLText;
            }

            function setSelect1() {
                const defaultText = filterModel.structureData.querySelector('anyTitle1').textContent;
                const data = filterModel.getStep1Data();

                select1.innerHTML = generatePullDownHTMLText(defaultText, data);
            }

            /**
             * Generate option when inactive
             * @param {string} text
             * @returns {string}
             */
            function getDisableOption(text) {
                return '<option value="" selected>' + text + '</option>';
            }

            function setSelect2() {
                // Deactivate if no item selected in step 2
                if (filterModel.currentSelectState1 === null) {
                    const data = filterModel.structureData.querySelector('anyTitle6');
                    const text = data ? data.textContent : '';

                    select2.innerHTML = getDisableOption(text);
                    select2.disabled = true;
                    selectWrap2.classList.add('-disabled');
                    // Deactivate to skip step 2
                } else if (filterModel.getStep2DisabledState()) {
                    const data = filterModel.structureData.querySelector('anyTitle9');
                    const text = data ? data.textContent : '';

                    select2.innerHTML = getDisableOption(text);
                    select2.disabled = true;
                    selectWrap2.classList.add('-disabled');
                    // Activate step 2
                } else {
                    const defaultText = filterModel.structureData.querySelector('anyTitle2').textContent;
                    const data = filterModel.getStep2Data();

                    select2.innerHTML = generatePullDownHTMLText(defaultText, data);
                    select2.disabled = false;
                    selectWrap2.classList.remove('-disabled');
                }
            }

            function setSelect3() {
                // Deactivate if no item selected in step 3
                if (filterModel.getStep3DisabledState()) {
                    const data = filterModel.structureData.querySelector('anyTitle10');
                    const text = data ? data.textContent : '';

                    select3.innerHTML = getDisableOption(text);
                    select3.disabled = true;
                    selectWrap3.classList.add('-disabled');
                    // Step 1 is not selected, or step 2 is required and not selected
                } else if (filterModel.currentSelectState1 === null || (!filterModel.getStep2DisabledState() && filterModel.currentSelectState2 === null)) {
                    const data = filterModel.structureData.querySelector('anyTitle7');
                    const text = data ? data.textContent : '';

                    select3.innerHTML = getDisableOption(text);
                    select3.disabled = true;
                    selectWrap3.classList.add('-disabled');
                    // Step 1 and step 2 are both selected（Activate step 3）
                } else {
                    const defaultText = filterModel.structureData.querySelector('anyTitle3').textContent;
                    const data = filterModel.getStep3Data();

                    select3.innerHTML = generatePullDownHTMLText(defaultText, data);
                    select3.disabled = false;
                    selectWrap3.classList.remove('-disabled');
                }
            }

            function onChangeSelect1() {
                filterModel.currentSelectState1 = select1.value;
                filterModel.currentSelectState2 = null;
                filterModel.currentSelectState3 = null;

                // Set 0th item forcedly to skip the second selection
                if (filterModel.getStep2DisabledState()) {
                    filterModel.currentSelectState2 = 0;
                }

                // Reset step 2, step 3, and result
                setSelect2();
                setSelect3();
                generateResult();
            }

            function onChangeSelect2() {
                filterModel.currentSelectState2 = select2.value;

                filterModel.currentSelectState3 = null;

                // Set 0th item forcedly to skip the third selection
                if (filterModel.getStep3DisabledState()) {
                    filterModel.currentSelectState3 = 0;
                }

                // Reset step 3 and result
                setSelect3();
                generateResult();
            }

            function onChangeSelect3() {
                filterModel.currentSelectState3 = select3.value;

                generateResult();
            }

            select1.addEventListener('change', onChangeSelect1);
            select2.addEventListener('change', onChangeSelect2);
            select3.addEventListener('change', onChangeSelect3);

            setSelect1();
            setSelect2();
            setSelect3();
        }

        /**
         * Build the markup for the filtering function
         */
        function createStructure(_root) {
            if (!filterModel.structureData || !filterModel.filterData) {
                return;
            }

            let html = FILTER_HTML_TEMPLATE;
            const replaceArray = [
                'anyTitle1',
                'anyTitle2',
                'anyTitle3',
                'anyTitle4',
                'anyTitle8'
            ];

            // Replace title text for each pulldown
            for (let i = 0, len = replaceArray.length; i < len; i++) {
                const key = replaceArray[i];
                const data = filterModel.structureData.querySelector(key);

                if (data) {
                    // Combine HTML template and XML
                    html = html.replace('{{' + key + '}}', data.textContent);
                }
            }

            _root.innerHTML = html;

            // Apply generic form function
            customModules.setInputFormItem(_root.querySelectorAll('.js-form-item'));

            setGeneralFilter(_root.querySelector('.js-filter-root'));
        }

        // Build the DOM once the two XMLs have been loaded
        function onLoadXMLComplete() {
            loadedXmlCount++;

            if (loadedXmlCount >= MAX_XML_COUNT) {
                createStructure(drillDownElement);
            }
        }

        fileLoader.loadXML(window.localData.drillDown.general.dataXml, function (xml) {
            filterModel.filterData = xml;
            onLoadXMLComplete();
        });
        fileLoader.loadXML(window.localData.drillDown.general.structureXml, function (xml) {
            filterModel.structureData = xml;
            onLoadXMLComplete();
        });
    }());

    // slider
    (function (roots) {
        if (roots.length === 0) {
            return;
        }

        const instances = [];

        function Slider(_root) {
            this._root = _root;
            this._body = _root.querySelector('.js-slider-body');
            this._item = _root.querySelectorAll('.js-slider-item');
            this._col = Number(_root.dataset.jsSliderCol);
            this._totalSlider = Math.ceil(this._item.length / this._col);
            this._ui = _root.querySelector('.js-slider-ui');
            this._prevBtn = _root.querySelector('.js-slider-prev');
            this._nextBtn = _root.querySelector('.js-slider-next');
            this._bullets = _root.querySelector('.js-slider-bullets');
            this._bulletBtns = null;
            this._currentIndex = 0;
            this._wasCurrentIndex = 0;
            this._translateXPercent = 0;
            this._isRun = false;

            for (let i = 0, len = this._item.length; i < len; i++) {
                this._item[i].dataset.jsSliderIndex = Math.floor(i / this._col);
            }
        }

        Slider.prototype = {
            setSlider: function () {
                for (let i = 0, len = this._item.length; i < len; i++) {
                    if (Number(this._item[i].dataset.jsSliderIndex) === this._currentIndex) {
                        this._item[i].classList.remove('invisible');
                    } else {
                        this._item[i].classList.add('invisible');
                    }
                }
                if (this._currentIndex !== 0) {
                    this._body.style.transform = 'translateX(' + this._translateXPercent + '%)';
                }

                this._ui.classList.remove('hidden');
            },
            removeSlider: function () {
                this._ui.classList.add('hidden');
                for (let i = 0, len = this._item.length; i < len; i++) {
                    this._item[i].classList.remove('invisible');
                }
                this._body.style = '';
            },
            /**
             * Executed when the media match status changes
             */
            setSliderMode: function () {
                if (this._item.length <= this._col) {
                    return;
                }
                if (matchMedia.matches) {
                    this.removeSlider();

                    return;
                }
                this.setSlider();
            },
            setBulletBtns: function () {
                this._bulletBtns = this._bullets.querySelectorAll('.js-slider-bullet-btn');
                for (let i = 0, len = this._bulletBtns.length; i < len; i++) {
                    this._bulletBtns[i].addEventListener('click', this.setSliderEvent.bind(this, null, i));
                }
            },
            changeCurrentBulletBtns: function () {
                this._bulletBtns[this._wasCurrentIndex].classList.remove('-current');
                this._bulletBtns[this._wasCurrentIndex].removeAttribute('aria-selected');
                this._bulletBtns[this._currentIndex].classList.add('-current');
                this._bulletBtns[this._currentIndex].setAttribute('aria-selected', 'true');
            },
            changeCurrent: function (directionNum, toIndex) {
                this._wasCurrentIndex = this._currentIndex;
                if (toIndex !== null) {
                    this._currentIndex = toIndex;

                    return;
                }
                this._currentIndex += directionNum;
            },
            /**
             * Perform the process to erase the button only for the maximum or minimum number of slides
             */
            changeShowSliderBtn: function () {
                const isMin = this._currentIndex === 0;
                const isMax = this._currentIndex === this._totalSlider - 1;

                if (isMin) {
                    this._prevBtn.classList.add('invisible');
                } else {
                    this._prevBtn.classList.remove('invisible');
                }

                if (isMax) {
                    this._nextBtn.classList.add('invisible');
                } else {
                    this._nextBtn.classList.remove('invisible');
                }
            },
            moveSlider: function () {
                this._translateXPercent = this._currentIndex * -100;

                for (let i = 0, len = this._item.length; i < len; i++) {
                    const itemIndexNum = Number(this._item[i].dataset.jsSliderIndex);

                    if (itemIndexNum !== this._wasCurrentIndex) {
                        this._item[i].classList.remove('invisible');
                    }
                }

                this._body.style.transform = 'translateX(' + this._translateXPercent + '%)';
            },
            completeMoveSlider: function () {
                if (!this._isRun) {
                    return;
                }
                for (let i = 0, len = this._item.length; i < len; i++) {
                    const itemIndexNum = Number(this._item[i].dataset.jsSliderIndex);

                    if (itemIndexNum !== this._currentIndex) {
                        this._item[i].classList.add('invisible');
                    }
                }
                this._isRun = false;
            },
            setSliderUI: function (index) {
                this._body.id = 'slider-body-' + index;
                //const bulletBtnText = window.localData.text.sliderBulletText || 'Show the nth slide';
                const bulletBtnText = 'Show the nth slide';
                let bulletBtn = '<button class="m-slider__bullet js-slider-bullet-btn -current" type="button" aria-selected="true" aria-controls="slider-body-' + index + '"><span class="m-slider__bullet-text">' + bulletBtnText.replace(' nth ', ' 1th ') + '</span></button>\n';

                for (let i = 1, len = this._totalSlider; i < len; i++) {
                    bulletBtn += '<button class="m-slider__bullet js-slider-bullet-btn" type="button" aria-controls="slider-body-' + index + '"><span class="m-slider__bullet-text">' + bulletBtnText.replace(' nth ', ' ' + (i + 1) + 'th ') + '</span></button>\n';
                }

                this._bullets.insertAdjacentHTML('beforeend', bulletBtn);
            },
            setSliderEvent: function (direction, toIndex) {
                if (toIndex === this._currentIndex || this._isRun) {
                    return;
                }
                this._isRun = true;
                let toDirection = direction;

                if (!direction) {
                    toDirection = toIndex > this._currentIndex ? 'left' : 'right';
                }
                this.changeCurrent(toDirection === 'left' ? 1 : -1, toIndex);
                this.changeCurrentBulletBtns();
                this.changeShowSliderBtn();
                this.moveSlider();
            },
            init: function (index) {
                if (this._item.length <= this._col) {
                    this._ui.classList.add('hidden');

                    return;
                }
                this.setSliderUI(index);
                this.setBulletBtns();
                this.changeShowSliderBtn();
                this._prevBtn.setAttribute('aria-controls', 'slider-body-' + index);
                this._nextBtn.setAttribute('aria-controls', 'slider-body-' + index);
                this._prevBtn.addEventListener('click', this.setSliderEvent.bind(this, 'right', null));
                this._nextBtn.addEventListener('click', this.setSliderEvent.bind(this, 'left', null));
                this._body.addEventListener('transitionend', this.completeMoveSlider.bind(this));
            }
        };

        function onChangeMatchMedia() {
            for (let i = 0, len = instances.length; i < len; i++) {
                instances[i].setSliderMode();
            }
        }

        for (let i = 0, len = roots.length; i < len; i++) {
            const slider = new Slider(roots.item(i));

            instances.push(slider);
            instances[i].init(i);
        }

        matchMedia.addListener(onChangeMatchMedia);
        onChangeMatchMedia();
    }(document.querySelectorAll('.js-slider-root')));

    // Digital marketing filter
    (function () {
        /* globals Muuri */
        const dmFilterElement = document.querySelector('.js-dm-filter-root');

        if (!dmFilterElement) {
            return;
        }

        function setDMFilter(_root) {
            const resultCountElement = _root.querySelector('.js-dm-count-text');
            const contentsWrapNode = _root.querySelector('.js-dm-contents');
            const muuri = new Muuri(contentsWrapNode);
            /** @type {NodeListOf<HTMLSelectElement>} */
            const selectNodeList = _root.querySelectorAll('.js-dm-filter-condition-select');
            const selectConditionLength = selectNodeList.length;
            const currentSelectState = {};
            /** @type {NodeListOf<HTMLInputElement>} */
            const checkboxNodeList = _root.querySelectorAll('.js-dm-filter-condition-check');
            const radioNodeList = _root.querySelectorAll('.js-dm-filter-condition-radio');
            const checkboxConditionLength = checkboxNodeList.length;
            const radioConditionLength = radioNodeList.length;
            let currentCheckState = [];
            let currentRadioState = [];
            const conditionKeyList = [];
            /** @type {HTMLButtonElement} */
            const triggerForOpen = _root.querySelector('.js-dm-filter-open');
            /** @type {HTMLButtonElement} */
            const triggerForClose = _root.querySelector('.js-dm-filter-close');
            /** @type {HTMLButtonElement} */
            const triggerForClearCondition = _root.querySelector('.js-dm-condition-clear');
            /** @type {HTMLButtonElement} */
            const triggerForSearch = _root.querySelector('.js-dm-condition-search');
            const changeEventObject = new CustomEvent('change');

            /**
             * Reflect the number of search results on the screen
             * @param number
             */
            function setTargetCount(number) {
                resultCountElement.textContent = number;
            }

            /**
             * Determine whether the conditions selected in the select box match
             * @param {DOMStringMap} dataset
             * @returns {boolean}
             */
            function checkForSelectCondition(dataset) {
                let result = true;

                for (let i = 0; i < selectConditionLength; i++) {
                    const key = conditionKeyList[i];
                    const selectedValue = currentSelectState[key];

                    if (selectedValue && dataset.hasOwnProperty(key)) {
                        const values = dataset[key].split(',');

                        for (let j = 0, len = values.length; j < len; j++) {
                            if (selectedValue && values[j] !== selectedValue) {
                                result = false;
                            }
                        }
                    }
                }

                return result;
            }

            /**
             * Determine whether the conditions checked in the check box match
             * @param {DOMStringMap} dataset
             * @returns {boolean}
             */
            function checkForCheckCondition(dataset) {
                let result = true;

                if (currentCheckState.length > 0) {
                    if (dataset.hasOwnProperty('feature')) {
                        const itemFeature = dataset.feature.split(',');

                        for (let i = 0, len = currentCheckState.length; i < len; i++) {
                            if (itemFeature.indexOf(currentCheckState[i]) === -1) {
                                result = false;
                            }
                        }
                    }
                }

                return result;
            }

            /**
             * Determine whether the conditions checked in the radio match
             * @param {DOMStringMap} dataset
             * @returns {boolean}
             */
            function checkForRadioCondition(dataset) {
                let result = true;

                if (currentRadioState.length > 0) {
                    if (dataset.hasOwnProperty('feature')) {
                        const itemFeature = dataset.feature.split(',');

                        for (let i = 0, len = currentRadioState.length; i < len; i++) {
                            if (itemFeature.indexOf(currentRadioState[i]) === -1) {
                                result = false;
                            }
                        }
                    }
                }

                return result;
            }

            /**
             * Run when conditions change
             */
            function onChangeCondition() {
                let count = 0;

                muuri.filter(function (item) {
                    let isShow;
                    const element = item.getElement();
                    const dataset = element.dataset;

                    // Verification of the select box
                    isShow = checkForSelectCondition(dataset);

                    // If the verification of the select box is passed, also verify the radio
                    if (isShow) {
                        isShow = checkForRadioCondition(dataset);
                    }

                    // If the verification of the radio is passed, also verify the check box
                    if (isShow) {
                        isShow = checkForCheckCondition(dataset);
                    }

                    if (isShow) {
                        count++;
                    }

                    return isShow;
                });

                setTargetCount(count);
            }

            /**
             * Update the search condition object based on the value of the select element
             * @param {HTMLSelectElement} select
             */
            function updateSelectState(select) {
                currentSelectState[select.name] = select.value;
            }

            /**
             * Run when the conditions of the select box are changed
             * @param {InputEvent} e
             */
            function onChangeSelectCondition(e) {
                const select = e.currentTarget;

                updateSelectState(select);

                // When using a PC, reflect the conditions immediately
                if (!matchMedia.matches) {
                    onChangeCondition();
                }
            }

            /**
             * Update checked objects based on the radio status
             * @param {HTMLInputElement} radio
             */
            function updateRadioState(radio) {
                const value = radio.value;
                if (radio.checked) {
                    currentRadioState = value;
                } else {
                    currentRadioState = currentRadioState.filter(function (_value) {
                        return _value !== value;
                    });
                }
            }

            /**
             * Run when the radio type conditions change
             * @param {InputEvent} e
             */
            function onChangeRadioCondition(e) {
                const radio = e.currentTarget;

                updateRadioState(radio);

                // PCの時は条件を即時反映
                if (!matchMedia.matches) {
                    onChangeCondition();
                }
            }

            /**
             * Update checked objects based on the check box status
             * @param {HTMLInputElement} checkbox
             */
            function updateCheckState(checkbox) {
                const value = checkbox.value;

                if (checkbox.checked) {
                    currentCheckState.push(value);
                } else {
                    currentCheckState = currentCheckState.filter(function (_value) {
                        return _value !== value;
                    });
                }
            }

            /**
             * Run when the check box type conditions change
             * @param {InputEvent} e
             */
            function onChangeCheckboxCondition(e) {
                const checkbox = e.currentTarget;

                updateCheckState(checkbox);

                // When using a PC, reflect the conditions immediately
                if (!matchMedia.matches) {
                    onChangeCondition();
                }
            }

            function openDrawer() {
                scrollController.lock();
                _root.style.height = screen.availHeight + 'px';
                _root.classList.add('-open');
            }

            function closeDrawer() {
                _root.style.height = '';
                scrollController.unlock();
                _root.classList.remove('-open');
            }

            /**
             * Clear all current search conditions
             */
            function clearCondition() {
                // Reset the select element
                for (let i = 0; i < selectConditionLength; i++) {
                    const select = selectNodeList.item(i);

                    select.selectedIndex = 0;
                    select.dispatchEvent(changeEventObject);
                }

                // Reset the radio element
                for (let i = 0; i < radioConditionLength; i++) {
                    const radio = radioNodeList.item(i);

                    radio.checked = false;
                    radio.dispatchEvent(changeEventObject);
                }

                // Reset the checkbox element
                for (let i = 0; i < checkboxConditionLength; i++) {
                    const checkbox = checkboxNodeList.item(i);

                    checkbox.checked = false;
                    checkbox.dispatchEvent(changeEventObject);
                }
            }

            function search() {
                onChangeCondition();
                closeDrawer();
            }

            function onChangeMatchMedia() {
                closeDrawer();
            }

            function init() {
                for (let i = 0; i < selectConditionLength; i++) {
                    const select = selectNodeList.item(i);
                    const key = select.name;

                    conditionKeyList.push(key);
                    currentSelectState[key] = '';
                    select.addEventListener('change', onChangeSelectCondition);
                }

                for (let i = 0; i < radioConditionLength; i++) {
                    const radio = radioNodeList.item(i);

                    radio.addEventListener('change', onChangeRadioCondition);
                }

                for (let i = 0; i < checkboxConditionLength; i++) {
                    const checkbox = checkboxNodeList.item(i);

                    checkbox.addEventListener('change', onChangeCheckboxCondition);
                }

                setTargetCount(muuri.getItems().length);
                muuri.layout();
                triggerForOpen.addEventListener('click', openDrawer);
                triggerForClose.addEventListener('click', closeDrawer);
                triggerForClearCondition.addEventListener('click', clearCondition);
                triggerForSearch.addEventListener('click', search);
                matchMedia.addListener(onChangeMatchMedia);
            }

            init();
        }

        setDMFilter(dmFilterElement);
    }());

    // carousel
    (function (roots) {
        if (roots.length === 0) {
            return;
        }
        const instances = [];

        function Carousel(_root) {
            this._root = _root;
            this._body = _root.querySelector('.js-carousel-body');
            this._item = _root.querySelectorAll('.js-carousel-item');
            this._totalItem = this._item.length;
            this._prevItem = null;
            this._nextItem = null;
            this._prevBtn = _root.querySelector('.js-carousel-prev');
            this._nextBtn = _root.querySelector('.js-carousel-next');
            this._bullets = _root.querySelector('.js-carousel-bullets');
            this._bulletBtns = null;
            this._direction = null;
            this._isAutoPlay = _root.dataset.jsCarouselAuto === 'true';
            this._autoPlayTimerId = null;
            this._interval = this._isAutoPlay ? 10000 : null;
            this._currentIndex = 0;
            this._wasCurrentIndex = 0;
            this._swipeStartX = 0;
            this._swipeMoveX = 0;
            this._swipeDiffPercent = 0;
            this._isSwipe = false;
            this._isShowPrev = false;
            this._isShowNext = false;
            this._isRun = false;
        }

        Carousel.prototype = {
            setCarouselUI: function (index) {
                this._body.id = 'carousel-body-' + index;
                //const bulletBtnText = window.localData.text.carouselBulletText || 'Show the nth item';
                const bulletBtnText = 'Show the nth item';
                let bulletBtn = '<button class="m-carousel__bullet js-carousel-bullet-btn -current" type="button" aria-selected="true" aria-controls="carousel-body-' + index + '"><span class="m-carousel__bullet-text">' + bulletBtnText.replace(' nth ', ' 1th ') + '</span></button>\n';

                for (let i = 1, len = this._totalItem; i < len; i++) {
                    bulletBtn += '<button class="m-carousel__bullet js-carousel-bullet-btn" type="button" aria-controls="carousel-body-' + index + '"><span class="m-carousel__bullet-text">' + bulletBtnText.replace(' nth ', ' ' + (i + 1) + 'th ') + '</span></button>\n';
                }

                this._bullets.insertAdjacentHTML('beforeend', bulletBtn);
            },
            setBulletBtns: function () {
                this._bulletBtns = this._bullets.querySelectorAll('.js-carousel-bullet-btn');
                for (let i = 0, len = this._bulletBtns.length; i < len; i++) {
                    this._bulletBtns[i].addEventListener('click', this.carouselEvent.bind(this, null, i, false));
                }
            },
            setPrevNextCarouselItems: function () {
                let prevIndex = this._currentIndex - 1;
                let nextIndex = this._currentIndex + 1;

                switch (this._currentIndex) {
                    case 0:
                        prevIndex = this._totalItem - 1;
                        nextIndex = this._currentIndex + 1;
                        break;
                    case this._totalItem - 1:
                        prevIndex = this._currentIndex - 1;
                        nextIndex = 0;
                        break;
                    default:
                        break;
                }

                this._prevItem = this._item[prevIndex];
                this._nextItem = this._item[nextIndex];
            },
            /**
             * Show previous carousel item
             */
            showPrevCarouselItems: function () {
                if (this._isSwipe) {
                    this._prevItem.classList.add('js-carousel-move-prev');
                    this._prevItem.classList.remove('hidden');

                    return;
                }
                this._item[this._currentIndex].classList.add('js-carousel-move-prev');
                this._item[this._currentIndex].classList.remove('hidden');
            },
            /**
             * Hide the previous carousel item only when the swipe operation did not move the carousel
             */
            hiddenSwipePrevCarouselItems: function () {
                this._prevItem.classList.add('hidden');
                this._prevItem.classList.remove('js-carousel-move-prev');
                this._prevItem.style.transform = '';
            },
            hiddenSwipePrevCarouselTwoItems: function () {
                this._prevItem.classList.remove('js-carousel-move-prev');
                this._prevItem.style.transform = '';
            },
            /**
             * Show next carousel item
             */
            showNextCarouselItems: function () {
                if (this._isSwipe) {
                    this._nextItem.classList.add('js-carousel-move-next');
                    this._nextItem.classList.remove('hidden');

                    return;
                }
                this._item[this._currentIndex].classList.add('js-carousel-move-next');
                this._item[this._currentIndex].classList.remove('hidden');
            },
            /**
             * Hide the next carousel item only when the swipe operation did not move the carousel
             */
            hiddenSwipeNextCarouselItems: function () {
                this._nextItem.classList.add('hidden');
                this._nextItem.classList.remove('js-carousel-move-next');
                this._nextItem.style.transform = '';
            },
            hiddenSwipeNextCarouselTwoItems: function () {
                this._nextItem.classList.remove('js-carousel-move-next');
                this._nextItem.style.transform = '';
            },
            /**
             * Carousel item switching: Divide the processing depending on whether or not it is a swipe operation
             */
            moveCarouselItems: function () {
                if (this._isSwipe) {
                    this._item[this._currentIndex].classList.add('js-carousel-move--quick');
                    this._item[this._wasCurrentIndex].classList.add('js-carousel-move--quick');
                    this._item[this._currentIndex].style.transform = 'translateX(0)';

                    switch (this._direction) {
                        case 'left':
                            this._item[this._wasCurrentIndex].style.transform = 'translateX(-100%)';
                            break;
                        case 'right':
                            this._item[this._wasCurrentIndex].style.transform = 'translateX(100%)';
                            break;
                        default:
                            break;
                    }

                    return;
                }

                this._item[this._wasCurrentIndex].classList.add('js-carousel-move-' + this._direction);
                this._item[this._currentIndex].classList.add('js-carousel-move-' + this._direction);
            },
            /**
             * After carousel switching occurs, delete the attribute added to the element
             */
            removeCarouselItemsAttr: function () {
                const showItem = this._direction === 'left' ? 'next' : 'prev';

                this._item[this._wasCurrentIndex].style.transform = '';
                this._item[this._currentIndex].style.transform = '';

                this._item[this._wasCurrentIndex].classList.add('hidden');
                this._item[this._wasCurrentIndex].classList.remove('js-carousel-move--quick');
                this._item[this._wasCurrentIndex].classList.remove('js-carousel-move-' + this._direction);
                this._item[this._wasCurrentIndex].classList.remove('js-carousel-move-' + showItem);
                this._item[this._currentIndex].classList.remove('js-carousel-move--quick');
                this._item[this._currentIndex].classList.remove('js-carousel-move-' + this._direction);
                this._item[this._currentIndex].classList.remove('js-carousel-move-' + showItem);
            },
            /**
             * Fire processing related to carousel switching
             * @param {boolean} isAutoPlayMove Whether to switch by automatic switching If it is not automatic switching, reset the timing of automatic switching
             */
            runCarouselItems: function (isAutoPlayMove) {
                if (!isAutoPlayMove) {
                    this.resetAutoPlay();
                }

                if (!this._isSwipe) {
                    switch (this._direction) {
                        case 'left':
                            this.showNextCarouselItems();
                            break;
                        case 'right':
                            this.showPrevCarouselItems();
                            break;
                        default:
                            break;
                    }
                }

                this.moveCarouselItems();
            },
            transitionEndCarouselMove: function () {
                if (!this._isRun) {
                    return;
                }

                this.removeCarouselItemsAttr();
                this.setPrevNextCarouselItems();
                this._isRun = false;
                this._isSwipe = false;
            },
            changeCurrentBulletBtns: function () {
                this._bulletBtns[this._wasCurrentIndex].classList.remove('-current');
                this._bulletBtns[this._wasCurrentIndex].removeAttribute('aria-selected');
                this._bulletBtns[this._currentIndex].classList.add('-current');
                this._bulletBtns[this._currentIndex].setAttribute('aria-selected', 'true');
            },
            changeCurrent: function (directionNum, toIndex) {
                this._wasCurrentIndex = this._currentIndex;
                if (toIndex !== null) {
                    this._currentIndex = toIndex;

                    return;
                }
                this._currentIndex += directionNum;

                switch (this._currentIndex) {
                    case -1:
                        this._currentIndex = this._totalItem - 1;
                        break;
                    case this._totalItem:
                        this._currentIndex = 0;
                        break;
                    default:
                        break;
                }
            },
            setAutoplay: function () {
                if (!this._isAutoPlay) {
                    return;
                }

                this._autoPlayTimerId = setInterval(this.carouselEvent.bind(this, 1, null, true), this._interval);
            },
            resetAutoPlay: function () {
                clearInterval(this._autoPlayTimerId);
                this.setAutoplay();
            },
            /**
             * Compare the current index with the previous index to determine where to move to.
             * @param  {number} directionNum Numerical value that determines the direction to move to Control unintended behavior when there are two carousel items
             * @param  {number} toIndex Index number for which carousel item to move to when the bullet button is pressed
             */
            setMoveDirection: function (directionNum, toIndex) {
                if (toIndex !== null) {
                    this._direction = toIndex > this._wasCurrentIndex ? 'left' : 'right';

                    return;
                }

                if (directionNum === 1 && this._currentIndex === 0 && this._wasCurrentIndex === this._totalItem - 1) {
                    this._direction = 'left';

                    return;
                } else if (directionNum === -1 && this._currentIndex === this._totalItem - 1 && this._wasCurrentIndex === 0) {
                    this._direction = 'right';

                    return;
                }

                this._direction = this._currentIndex > this._wasCurrentIndex ? 'left' : 'right';
            },
            getSwipeMoveDirectionNum: function (swipeMoveXPercent) {
                if (swipeMoveXPercent < -33) {
                    return 1;
                } else if (swipeMoveXPercent > 33) {
                    return -1;
                }

                return null;
            },
            swipeTouchStart: function (e) {
                this._isSwipe = true;
                this._isRun = true;
                this._swipeStartX = e.touches[0].pageX;
            },
            swipeTouchMove: function (e) {
                if (this._swipeDiffPercent > 100 || this._swipeDiffPercent < -100) {
                    return;
                }
                this._swipeMoveX = e.touches[0].pageX;
                this._swipeDiffPercent = (this._swipeMoveX - this._swipeStartX) / this._root.offsetWidth * 100;
                this._item[this._currentIndex].style.transform = 'translateX(' + this._swipeDiffPercent + '%)';

                // Switch left or right placement of carousel items according to the swipe position
                if (this._swipeDiffPercent > 0) {
                    if (!this._isShowPrev) {
                        this.showPrevCarouselItems();
                        if (this._totalItem == 2) {
                            this.hiddenSwipeNextCarouselTwoItems();
                        }
                        else {
                            this.hiddenSwipeNextCarouselItems();
                        }
                        this._isShowPrev = true;
                        this._isShowNext = false;
                    }
                    this._prevItem.style.transform = 'translateX(' + (-100 + this._swipeDiffPercent) + '%)';
                } else if (this._swipeDiffPercent < 0) {
                    if (!this._isShowNext) {
                        this.showNextCarouselItems();
                        if (this._totalItem == 2) {
                            this.hiddenSwipePrevCarouselTwoItems();
                        }
                        else {
                            this.hiddenSwipePrevCarouselItems();
                        }
                        this._isShowPrev = false;
                        this._isShowNext = true;
                    }
                    this._nextItem.style.transform = 'translateX(' + (100 + this._swipeDiffPercent) + '%)';
                }

            },
            swipeTouchEnd: function () {
                const directionNum = this.getSwipeMoveDirectionNum(this._swipeDiffPercent);

                this._isRun = false;
                this._isShowPrev = false;
                this._isShowNext = false;
                this._swipeDiffPercent = 0;

                if (directionNum === null) {
                    this.hiddenSwipeNextCarouselItems();
                    this.hiddenSwipePrevCarouselItems();
                    this._item[this._currentIndex].style.transform = '';

                    return;
                }
                this.carouselEvent(directionNum, null, false);
            },
            /**
             * Fire processing related to carousel switching
             * @param {number} directionNum Number in the direction in which the carousel item switches 1 for right, -1 for left
             * @param {number} toIndex Index for which carousel item to switch to when the bullet button is pressed
             * @param {boolean} isAutoPlayMove Whether to switch by automatic switching
             */
            carouselEvent: function (directionNum, toIndex, isAutoPlayMove) {
                if (toIndex === this._currentIndex || this._isRun) {
                    return;
                }
                this._isRun = true;

                this.changeCurrent(directionNum, toIndex);
                this.changeCurrentBulletBtns();
                this.setMoveDirection(directionNum, toIndex);
                this.runCarouselItems(isAutoPlayMove);
            },
            init: function (index) {
                if (this._totalItem < 2) {
                    this._prevBtn.classList.add('hidden');
                    this._nextBtn.classList.add('hidden');

                    return;
                }
                this.setPrevNextCarouselItems();
                this.setCarouselUI(index);
                this.setBulletBtns();
                this.setAutoplay();
                this._prevBtn.setAttribute('aria-controls', 'carousel-body-' + index);
                this._nextBtn.setAttribute('aria-controls', 'carousel-body-' + index);
                this._prevBtn.addEventListener('click', this.carouselEvent.bind(this, -1, null, false));
                this._nextBtn.addEventListener('click', this.carouselEvent.bind(this, 1, null, false));

                for (let i = 0, len = this._totalItem; i < len; i++) {
                    if (i !== this._currentIndex) {
                        this._item[i].classList.add('hidden');
                    }
                    this._item[i].addEventListener('animationend', this.transitionEndCarouselMove.bind(this));
                    this._item[i].addEventListener('transitionend', this.transitionEndCarouselMove.bind(this));
                    this._item[i].addEventListener('touchstart', this.swipeTouchStart.bind(this));
                    this._item[i].addEventListener('touchmove', this.swipeTouchMove.bind(this));
                    this._item[i].addEventListener('touchend', this.swipeTouchEnd.bind(this));
                }
            }
        };

        for (let i = 0, len = roots.length; i < len; i++) {
            const carousel = new Carousel(roots.item(i));

            instances.push(carousel);
            instances[i].init(i);
        }
    }(document.querySelectorAll('.js-carousel-root')));

    // Return to top of page: Switch link display
    (function () {
        const pageTopBtn = document.querySelector('.js-page-top-btn');
        const parent = pageTopBtn ? pageTopBtn.parentElement : document.body;

        if (!pageTopBtn) {
            return;
        }

        function onScroll() {
            const scrollTop = scrollController.scrollPositionTop;
            const windowHeight = window.innerHeight;
            const threshold = scrollTop / windowHeight;
            const clientRect = parent.getBoundingClientRect();

            if (threshold > 0.5) {
                pageTopBtn.classList.add('-show');

                // If you can see the bottom of the parent, add a class for sticky
                if (windowHeight >= clientRect.bottom) {
                    pageTopBtn.classList.add('-scrollOver');
                } else {
                    pageTopBtn.classList.remove('-scrollOver');
                }
            } else {
                pageTopBtn.classList.remove('-show');
            }
        }

        scrollController.subscribeScrollEvent();
        DOCUMENT_ELEMENT.addEventListener(SCROLL_EVENT_NAME, onScroll);
        onScroll();
    }());
}(function () {
    'use strict';

    /* eslint-disable */
    // https://developer.mozilla.org/en-US/docs/Web/API/CustomEvent/CustomEvent#Polyfill
    (function () {

        if (typeof window.CustomEvent === "function") return false;

        function CustomEvent(event, params) {
            params = params || { bubbles: false, cancelable: false, detail: null };
            var evt = document.createEvent('CustomEvent');
            evt.initCustomEvent(event, params.bubbles, params.cancelable, params.detail);
            return evt;
        }

        window.CustomEvent = CustomEvent;
    })();

    // https://developer.mozilla.org/ja/docs/Web/API/Element/closest#Polyfill
    if (!Element.prototype.matches) {
        Element.prototype.matches = Element.prototype.msMatchesSelector ||
            Element.prototype.webkitMatchesSelector;
    }
    if (!Element.prototype.closest) {
        Element.prototype.closest = function (s) {
            var el = this;

            do {
                if (Element.prototype.matches.call(el, s)) return el;
                el = el.parentElement || el.parentNode;
            } while (el !== null && el.nodeType === 1);
            return null;
        };
    }

    // https://unpkg.com/smoothscroll-polyfill@0.4.4/dist/smoothscroll.min.js

    /**
     * The MIT License (MIT)
     *
     * Copyright (c) 2013 Dustan Kasten
     *
     * Permission is hereby granted, free of charge, to any person obtaining a copy of
     * this software and associated documentation files (the "Software"), to deal in
     * the Software without restriction, including without limitation the rights to
     * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
     * the Software, and to permit persons to whom the Software is furnished to do so,
     * subject to the following conditions:
     *
     * The above copyright notice and this permission notice shall be included in all
     * copies or substantial portions of the Software.
     *
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
     * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
     * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
     * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
     * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
     */
    (function () {
        !function () { "use strict"; function o() { var o = window, t = document; if (!("scrollBehavior" in t.documentElement.style && !0 !== o.__forceSmoothScrollPolyfill__)) { var l, e = o.HTMLElement || o.Element, r = 468, i = { scroll: o.scroll || o.scrollTo, scrollBy: o.scrollBy, elementScroll: e.prototype.scroll || n, scrollIntoView: e.prototype.scrollIntoView }, s = o.performance && o.performance.now ? o.performance.now.bind(o.performance) : Date.now, c = (l = o.navigator.userAgent, new RegExp(["MSIE ", "Trident/", "Edge/"].join("|")).test(l) ? 1 : 0); o.scroll = o.scrollTo = function () { void 0 !== arguments[0] && (!0 !== f(arguments[0]) ? h.call(o, t.body, void 0 !== arguments[0].left ? ~~arguments[0].left : o.scrollX || o.pageXOffset, void 0 !== arguments[0].top ? ~~arguments[0].top : o.scrollY || o.pageYOffset) : i.scroll.call(o, void 0 !== arguments[0].left ? arguments[0].left : "object" != typeof arguments[0] ? arguments[0] : o.scrollX || o.pageXOffset, void 0 !== arguments[0].top ? arguments[0].top : void 0 !== arguments[1] ? arguments[1] : o.scrollY || o.pageYOffset)) }, o.scrollBy = function () { void 0 !== arguments[0] && (f(arguments[0]) ? i.scrollBy.call(o, void 0 !== arguments[0].left ? arguments[0].left : "object" != typeof arguments[0] ? arguments[0] : 0, void 0 !== arguments[0].top ? arguments[0].top : void 0 !== arguments[1] ? arguments[1] : 0) : h.call(o, t.body, ~~arguments[0].left + (o.scrollX || o.pageXOffset), ~~arguments[0].top + (o.scrollY || o.pageYOffset))) }, e.prototype.scroll = e.prototype.scrollTo = function () { if (void 0 !== arguments[0]) if (!0 !== f(arguments[0])) { var o = arguments[0].left, t = arguments[0].top; h.call(this, this, void 0 === o ? this.scrollLeft : ~~o, void 0 === t ? this.scrollTop : ~~t) } else { if ("number" == typeof arguments[0] && void 0 === arguments[1]) throw new SyntaxError("Value could not be converted"); i.elementScroll.call(this, void 0 !== arguments[0].left ? ~~arguments[0].left : "object" != typeof arguments[0] ? ~~arguments[0] : this.scrollLeft, void 0 !== arguments[0].top ? ~~arguments[0].top : void 0 !== arguments[1] ? ~~arguments[1] : this.scrollTop) } }, e.prototype.scrollBy = function () { void 0 !== arguments[0] && (!0 !== f(arguments[0]) ? this.scroll({ left: ~~arguments[0].left + this.scrollLeft, top: ~~arguments[0].top + this.scrollTop, behavior: arguments[0].behavior }) : i.elementScroll.call(this, void 0 !== arguments[0].left ? ~~arguments[0].left + this.scrollLeft : ~~arguments[0] + this.scrollLeft, void 0 !== arguments[0].top ? ~~arguments[0].top + this.scrollTop : ~~arguments[1] + this.scrollTop)) }, e.prototype.scrollIntoView = function () { if (!0 !== f(arguments[0])) { var l = function (o) { for (; o !== t.body && !1 === (e = p(l = o, "Y") && a(l, "Y"), r = p(l, "X") && a(l, "X"), e || r);)o = o.parentNode || o.host; var l, e, r; return o }(this), e = l.getBoundingClientRect(), r = this.getBoundingClientRect(); l !== t.body ? (h.call(this, l, l.scrollLeft + r.left - e.left, l.scrollTop + r.top - e.top), "fixed" !== o.getComputedStyle(l).position && o.scrollBy({ left: e.left, top: e.top, behavior: "smooth" })) : o.scrollBy({ left: r.left, top: r.top, behavior: "smooth" }) } else i.scrollIntoView.call(this, void 0 === arguments[0] || arguments[0]) } } function n(o, t) { this.scrollLeft = o, this.scrollTop = t } function f(o) { if (null === o || "object" != typeof o || void 0 === o.behavior || "auto" === o.behavior || "instant" === o.behavior) return !0; if ("object" == typeof o && "smooth" === o.behavior) return !1; throw new TypeError("behavior member of ScrollOptions " + o.behavior + " is not a valid value for enumeration ScrollBehavior.") } function p(o, t) { return "Y" === t ? o.clientHeight + c < o.scrollHeight : "X" === t ? o.clientWidth + c < o.scrollWidth : void 0 } function a(t, l) { var e = o.getComputedStyle(t, null)["overflow" + l]; return "auto" === e || "scroll" === e } function d(t) { var l, e, i, c, n = (s() - t.startTime) / r; c = n = n > 1 ? 1 : n, l = .5 * (1 - Math.cos(Math.PI * c)), e = t.startX + (t.x - t.startX) * l, i = t.startY + (t.y - t.startY) * l, t.method.call(t.scrollable, e, i), e === t.x && i === t.y || o.requestAnimationFrame(d.bind(o, t)) } function h(l, e, r) { var c, f, p, a, h = s(); l === t.body ? (c = o, f = o.scrollX || o.pageXOffset, p = o.scrollY || o.pageYOffset, a = i.scroll) : (c = l, f = l.scrollLeft, p = l.scrollTop, a = n), d({ scrollable: c, method: a, startTime: h, startX: f, startY: p, x: e, y: r }) } } "object" == typeof exports && "undefined" != typeof module ? module.exports = { polyfill: o } : o() }();
    }());

    // https://github.com/scottjehl/picturefill

    /*! picturefill - v3.0.2 - 2016-02-12
     * https://scottjehl.github.io/picturefill/
     * Copyright (c) 2016 https://github.com/scottjehl/picturefill/blob/master/Authors.txt; Licensed MIT
     */
    (function () {
        !function (a) { var b = navigator.userAgent; a.HTMLPictureElement && /ecko/.test(b) && b.match(/rv\:(\d+)/) && RegExp.$1 < 45 && addEventListener("resize", function () { var b, c = document.createElement("source"), d = function (a) { var b, d, e = a.parentNode; "PICTURE" === e.nodeName.toUpperCase() ? (b = c.cloneNode(), e.insertBefore(b, e.firstElementChild), setTimeout(function () { e.removeChild(b) })) : (!a._pfLastSize || a.offsetWidth > a._pfLastSize) && (a._pfLastSize = a.offsetWidth, d = a.sizes, a.sizes += ",100vw", setTimeout(function () { a.sizes = d })) }, e = function () { var a, b = document.querySelectorAll("picture > img, img[srcset][sizes]"); for (a = 0; a < b.length; a++)d(b[a]) }, f = function () { clearTimeout(b), b = setTimeout(e, 99) }, g = a.matchMedia && matchMedia("(orientation: landscape)"), h = function () { f(), g && g.addListener && g.addListener(f) }; return c.srcset = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==", /^[c|i]|d$/.test(document.readyState || "") ? h() : document.addEventListener("DOMContentLoaded", h), f }()) }(window), function (a, b, c) { "use strict"; function d(a) { return " " === a || "	" === a || "\n" === a || "\f" === a || "\r" === a } function e(b, c) { var d = new a.Image; return d.onerror = function () { A[b] = !1, ba() }, d.onload = function () { A[b] = 1 === d.width, ba() }, d.src = c, "pending" } function f() { M = !1, P = a.devicePixelRatio, N = {}, O = {}, s.DPR = P || 1, Q.width = Math.max(a.innerWidth || 0, z.clientWidth), Q.height = Math.max(a.innerHeight || 0, z.clientHeight), Q.vw = Q.width / 100, Q.vh = Q.height / 100, r = [Q.height, Q.width, P].join("-"), Q.em = s.getEmValue(), Q.rem = Q.em } function g(a, b, c, d) { var e, f, g, h; return "saveData" === B.algorithm ? a > 2.7 ? h = c + 1 : (f = b - c, e = Math.pow(a - .6, 1.5), g = f * e, d && (g += .1 * e), h = a + g) : h = c > 1 ? Math.sqrt(a * b) : a, h > c } function h(a) { var b, c = s.getSet(a), d = !1; "pending" !== c && (d = r, c && (b = s.setRes(c), s.applySetCandidate(b, a))), a[s.ns].evaled = d } function i(a, b) { return a.res - b.res } function j(a, b, c) { var d; return !c && b && (c = a[s.ns].sets, c = c && c[c.length - 1]), d = k(b, c), d && (b = s.makeUrl(b), a[s.ns].curSrc = b, a[s.ns].curCan = d, d.res || aa(d, d.set.sizes)), d } function k(a, b) { var c, d, e; if (a && b) for (e = s.parseSet(b), a = s.makeUrl(a), c = 0; c < e.length; c++)if (a === s.makeUrl(e[c].url)) { d = e[c]; break } return d } function l(a, b) { var c, d, e, f, g = a.getElementsByTagName("source"); for (c = 0, d = g.length; d > c; c++)e = g[c], e[s.ns] = !0, f = e.getAttribute("srcset"), f && b.push({ srcset: f, media: e.getAttribute("media"), type: e.getAttribute("type"), sizes: e.getAttribute("sizes") }) } function m(a, b) { function c(b) { var c, d = b.exec(a.substring(m)); return d ? (c = d[0], m += c.length, c) : void 0 } function e() { var a, c, d, e, f, i, j, k, l, m = !1, o = {}; for (e = 0; e < h.length; e++)f = h[e], i = f[f.length - 1], j = f.substring(0, f.length - 1), k = parseInt(j, 10), l = parseFloat(j), X.test(j) && "w" === i ? ((a || c) && (m = !0), 0 === k ? m = !0 : a = k) : Y.test(j) && "x" === i ? ((a || c || d) && (m = !0), 0 > l ? m = !0 : c = l) : X.test(j) && "h" === i ? ((d || c) && (m = !0), 0 === k ? m = !0 : d = k) : m = !0; m || (o.url = g, a && (o.w = a), c && (o.d = c), d && (o.h = d), d || c || a || (o.d = 1), 1 === o.d && (b.has1x = !0), o.set = b, n.push(o)) } function f() { for (c(T), i = "", j = "in descriptor"; ;) { if (k = a.charAt(m), "in descriptor" === j) if (d(k)) i && (h.push(i), i = "", j = "after descriptor"); else { if ("," === k) return m += 1, i && h.push(i), void e(); if ("(" === k) i += k, j = "in parens"; else { if ("" === k) return i && h.push(i), void e(); i += k } } else if ("in parens" === j) if (")" === k) i += k, j = "in descriptor"; else { if ("" === k) return h.push(i), void e(); i += k } else if ("after descriptor" === j) if (d(k)); else { if ("" === k) return void e(); j = "in descriptor", m -= 1 } m += 1 } } for (var g, h, i, j, k, l = a.length, m = 0, n = []; ;) { if (c(U), m >= l) return n; g = c(V), h = [], "," === g.slice(-1) ? (g = g.replace(W, ""), e()) : f() } } function n(a) { function b(a) { function b() { f && (g.push(f), f = "") } function c() { g[0] && (h.push(g), g = []) } for (var e, f = "", g = [], h = [], i = 0, j = 0, k = !1; ;) { if (e = a.charAt(j), "" === e) return b(), c(), h; if (k) { if ("*" === e && "/" === a[j + 1]) { k = !1, j += 2, b(); continue } j += 1 } else { if (d(e)) { if (a.charAt(j - 1) && d(a.charAt(j - 1)) || !f) { j += 1; continue } if (0 === i) { b(), j += 1; continue } e = " " } else if ("(" === e) i += 1; else if (")" === e) i -= 1; else { if ("," === e) { b(), c(), j += 1; continue } if ("/" === e && "*" === a.charAt(j + 1)) { k = !0, j += 2; continue } } f += e, j += 1 } } } function c(a) { return k.test(a) && parseFloat(a) >= 0 ? !0 : l.test(a) ? !0 : "0" === a || "-0" === a || "+0" === a ? !0 : !1 } var e, f, g, h, i, j, k = /^(?:[+-]?[0-9]+|[0-9]*\.[0-9]+)(?:[eE][+-]?[0-9]+)?(?:ch|cm|em|ex|in|mm|pc|pt|px|rem|vh|vmin|vmax|vw)$/i, l = /^calc\((?:[0-9a-z \.\+\-\*\/\(\)]+)\)$/i; for (f = b(a), g = f.length, e = 0; g > e; e++)if (h = f[e], i = h[h.length - 1], c(i)) { if (j = i, h.pop(), 0 === h.length) return j; if (h = h.join(" "), s.matchesMedia(h)) return j } return "100vw" } b.createElement("picture"); var o, p, q, r, s = {}, t = !1, u = function () { }, v = b.createElement("img"), w = v.getAttribute, x = v.setAttribute, y = v.removeAttribute, z = b.documentElement, A = {}, B = { algorithm: "" }, C = "data-pfsrc", D = C + "set", E = navigator.userAgent, F = /rident/.test(E) || /ecko/.test(E) && E.match(/rv\:(\d+)/) && RegExp.$1 > 35, G = "currentSrc", H = /\s+\+?\d+(e\d+)?w/, I = /(\([^)]+\))?\s*(.+)/, J = a.picturefillCFG, K = "position:absolute;left:0;visibility:hidden;display:block;padding:0;border:none;font-size:1em;width:1em;overflow:hidden;clip:rect(0px, 0px, 0px, 0px)", L = "font-size:100%!important;", M = !0, N = {}, O = {}, P = a.devicePixelRatio, Q = { px: 1, "in": 96 }, R = b.createElement("a"), S = !1, T = /^[ \t\n\r\u000c]+/, U = /^[, \t\n\r\u000c]+/, V = /^[^ \t\n\r\u000c]+/, W = /[,]+$/, X = /^\d+$/, Y = /^-?(?:[0-9]+|[0-9]*\.[0-9]+)(?:[eE][+-]?[0-9]+)?$/, Z = function (a, b, c, d) { a.addEventListener ? a.addEventListener(b, c, d || !1) : a.attachEvent && a.attachEvent("on" + b, c) }, $ = function (a) { var b = {}; return function (c) { return c in b || (b[c] = a(c)), b[c] } }, _ = function () { var a = /^([\d\.]+)(em|vw|px)$/, b = function () { for (var a = arguments, b = 0, c = a[0]; ++b in a;)c = c.replace(a[b], a[++b]); return c }, c = $(function (a) { return "return " + b((a || "").toLowerCase(), /\band\b/g, "&&", /,/g, "||", /min-([a-z-\s]+):/g, "e.$1>=", /max-([a-z-\s]+):/g, "e.$1<=", /calc([^)]+)/g, "($1)", /(\d+[\.]*[\d]*)([a-z]+)/g, "($1 * e.$2)", /^(?!(e.[a-z]|[0-9\.&=|><\+\-\*\(\)\/])).*/gi, "") + ";" }); return function (b, d) { var e; if (!(b in N)) if (N[b] = !1, d && (e = b.match(a))) N[b] = e[1] * Q[e[2]]; else try { N[b] = new Function("e", c(b))(Q) } catch (f) { } return N[b] } }(), aa = function (a, b) { return a.w ? (a.cWidth = s.calcListLength(b || "100vw"), a.res = a.w / a.cWidth) : a.res = a.d, a }, ba = function (a) { if (t) { var c, d, e, f = a || {}; if (f.elements && 1 === f.elements.nodeType && ("IMG" === f.elements.nodeName.toUpperCase() ? f.elements = [f.elements] : (f.context = f.elements, f.elements = null)), c = f.elements || s.qsa(f.context || b, f.reevaluate || f.reselect ? s.sel : s.selShort), e = c.length) { for (s.setupRun(f), S = !0, d = 0; e > d; d++)s.fillImg(c[d], f); s.teardownRun(f) } } }; o = a.console && console.warn ? function (a) { console.warn(a) } : u, G in v || (G = "src"), A["image/jpeg"] = !0, A["image/gif"] = !0, A["image/png"] = !0, A["image/svg+xml"] = b.implementation.hasFeature("http://www.w3.org/TR/SVG11/feature#Image", "1.1"), s.ns = ("pf" + (new Date).getTime()).substr(0, 9), s.supSrcset = "srcset" in v, s.supSizes = "sizes" in v, s.supPicture = !!a.HTMLPictureElement, s.supSrcset && s.supPicture && !s.supSizes && !function (a) { v.srcset = "data:,a", a.src = "data:,a", s.supSrcset = v.complete === a.complete, s.supPicture = s.supSrcset && s.supPicture }(b.createElement("img")), s.supSrcset && !s.supSizes ? !function () { var a = "data:image/gif;base64,R0lGODlhAgABAPAAAP///wAAACH5BAAAAAAALAAAAAACAAEAAAICBAoAOw==", c = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==", d = b.createElement("img"), e = function () { var a = d.width; 2 === a && (s.supSizes = !0), q = s.supSrcset && !s.supSizes, t = !0, setTimeout(ba) }; d.onload = e, d.onerror = e, d.setAttribute("sizes", "9px"), d.srcset = c + " 1w," + a + " 9w", d.src = c }() : t = !0, s.selShort = "picture>img,img[srcset]", s.sel = s.selShort, s.cfg = B, s.DPR = P || 1, s.u = Q, s.types = A, s.setSize = u, s.makeUrl = $(function (a) { return R.href = a, R.href }), s.qsa = function (a, b) { return "querySelector" in a ? a.querySelectorAll(b) : [] }, s.matchesMedia = function () { return a.matchMedia && (matchMedia("(min-width: 0.1em)") || {}).matches ? s.matchesMedia = function (a) { return !a || matchMedia(a).matches } : s.matchesMedia = s.mMQ, s.matchesMedia.apply(this, arguments) }, s.mMQ = function (a) { return a ? _(a) : !0 }, s.calcLength = function (a) { var b = _(a, !0) || !1; return 0 > b && (b = !1), b }, s.supportsType = function (a) { return a ? A[a] : !0 }, s.parseSize = $(function (a) { var b = (a || "").match(I); return { media: b && b[1], length: b && b[2] } }), s.parseSet = function (a) { return a.cands || (a.cands = m(a.srcset, a)), a.cands }, s.getEmValue = function () { var a; if (!p && (a = b.body)) { var c = b.createElement("div"), d = z.style.cssText, e = a.style.cssText; c.style.cssText = K, z.style.cssText = L, a.style.cssText = L, a.appendChild(c), p = c.offsetWidth, a.removeChild(c), p = parseFloat(p, 10), z.style.cssText = d, a.style.cssText = e } return p || 16 }, s.calcListLength = function (a) { if (!(a in O) || B.uT) { var b = s.calcLength(n(a)); O[a] = b ? b : Q.width } return O[a] }, s.setRes = function (a) { var b; if (a) { b = s.parseSet(a); for (var c = 0, d = b.length; d > c; c++)aa(b[c], a.sizes) } return b }, s.setRes.res = aa, s.applySetCandidate = function (a, b) { if (a.length) { var c, d, e, f, h, k, l, m, n, o = b[s.ns], p = s.DPR; if (k = o.curSrc || b[G], l = o.curCan || j(b, k, a[0].set), l && l.set === a[0].set && (n = F && !b.complete && l.res - .1 > p, n || (l.cached = !0, l.res >= p && (h = l))), !h) for (a.sort(i), f = a.length, h = a[f - 1], d = 0; f > d; d++)if (c = a[d], c.res >= p) { e = d - 1, h = a[e] && (n || k !== s.makeUrl(c.url)) && g(a[e].res, c.res, p, a[e].cached) ? a[e] : c; break } h && (m = s.makeUrl(h.url), o.curSrc = m, o.curCan = h, m !== k && s.setSrc(b, h), s.setSize(b)) } }, s.setSrc = function (a, b) { var c; a.src = b.url, "image/svg+xml" === b.set.type && (c = a.style.width, a.style.width = a.offsetWidth + 1 + "px", a.offsetWidth + 1 && (a.style.width = c)) }, s.getSet = function (a) { var b, c, d, e = !1, f = a[s.ns].sets; for (b = 0; b < f.length && !e; b++)if (c = f[b], c.srcset && s.matchesMedia(c.media) && (d = s.supportsType(c.type))) { "pending" === d && (c = d), e = c; break } return e }, s.parseSets = function (a, b, d) { var e, f, g, h, i = b && "PICTURE" === b.nodeName.toUpperCase(), j = a[s.ns]; (j.src === c || d.src) && (j.src = w.call(a, "src"), j.src ? x.call(a, C, j.src) : y.call(a, C)), (j.srcset === c || d.srcset || !s.supSrcset || a.srcset) && (e = w.call(a, "srcset"), j.srcset = e, h = !0), j.sets = [], i && (j.pic = !0, l(b, j.sets)), j.srcset ? (f = { srcset: j.srcset, sizes: w.call(a, "sizes") }, j.sets.push(f), g = (q || j.src) && H.test(j.srcset || ""), g || !j.src || k(j.src, f) || f.has1x || (f.srcset += ", " + j.src, f.cands.push({ url: j.src, d: 1, set: f }))) : j.src && j.sets.push({ srcset: j.src, sizes: null }), j.curCan = null, j.curSrc = c, j.supported = !(i || f && !s.supSrcset || g && !s.supSizes), h && s.supSrcset && !j.supported && (e ? (x.call(a, D, e), a.srcset = "") : y.call(a, D)), j.supported && !j.srcset && (!j.src && a.src || a.src !== s.makeUrl(j.src)) && (null === j.src ? a.removeAttribute("src") : a.src = j.src), j.parsed = !0 }, s.fillImg = function (a, b) { var c, d = b.reselect || b.reevaluate; a[s.ns] || (a[s.ns] = {}), c = a[s.ns], (d || c.evaled !== r) && ((!c.parsed || b.reevaluate) && s.parseSets(a, a.parentNode, b), c.supported ? c.evaled = r : h(a)) }, s.setupRun = function () { (!S || M || P !== a.devicePixelRatio) && f() }, s.supPicture ? (ba = u, s.fillImg = u): !function(){ var c, d = a.attachEvent ? /d$|^c/ : /d$|^c|^i/, e = function () { var a = b.readyState || ""; f = setTimeout(e, "loading" === a ? 200 : 999), b.body && (s.fillImgs(), c = c || d.test(a), c && clearTimeout(f)) }, f = setTimeout(e, b.body ? 9 : 99), g = function (a, b) { var c, d, e = function () { var f = new Date - d; b > f ? c = setTimeout(e, b - f) : (c = null, a()) }; return function () { d = new Date, c || (c = setTimeout(e, b)) } }, h = z.clientHeight, i = function () { M = Math.max(a.innerWidth || 0, z.clientWidth) !== Q.width || z.clientHeight !== h, h = z.clientHeight, M && s.fillImgs() }; Z(a, "resize", g(i, 99)), Z(b, "readystatechange", e) }(), s.picturefill = ba, s.fillImgs = ba, s.teardownRun = u, ba._ = s, a.picturefillCFG = { pf: s, push: function (a) { var b = a.shift(); "function" == typeof s[b] ? s[b].apply(s, a) : (B[b] = a[0], S && s.fillImgs({ reselect: !0 })) } }; for (; J && J.length;)a.picturefillCFG.push(J.shift()); a.picturefill = ba, "object" == typeof module && "object" == typeof module.exports ? module.exports = ba : "function" == typeof define && define.amd && define("picturefill", function () { return ba }), s.supPicture || (A["image/webp"] = e("image/webp", "data:image/webp;base64,UklGRkoAAABXRUJQVlA4WAoAAAAQAAAAAAAAAAAAQUxQSAwAAAABBxAR/Q9ERP8DAABWUDggGAAAADABAJ0BKgEAAQADADQlpAADcAD++/1QAA==")) }(window, document);
    }());

    // https://github.com/fregante/object-fit-images

    /*! npm.im/object-fit-images 3.2.4
    * The MIT License (MIT)
    * Copyright (c) Federico Brigante <opensource@bfred.it> (bfred.it)

    * Permission is hereby granted, free of charge, to any person obtaining a copy
    * of this software and associated documentation files (the "Software"), to deal
    * in the Software without restriction, including without limitation the rights
    * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    * copies of the Software, and to permit persons to whom the Software is
    * furnished to do so, subject to the following conditions:

    * The above copyright notice and this permission notice shall be included in
    * all copies or substantial portions of the Software.

    * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    * THE SOFTWARE.
    */
    (function () {
        var objectFitImages = function () { "use strict"; function t(t, e) { return "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='" + t + "' height='" + e + "'%3E%3C/svg%3E" } function e(t) { if (t.srcset && !p && window.picturefill) { var e = window.picturefill._; t[e.ns] && t[e.ns].evaled || e.fillImg(t, { reselect: !0 }), t[e.ns].curSrc || (t[e.ns].supported = !1, e.fillImg(t, { reselect: !0 })), t.currentSrc = t[e.ns].curSrc || t.src } } function i(t) { for (var e, i = getComputedStyle(t).fontFamily, r = {}; null !== (e = u.exec(i));)r[e[1]] = e[2]; return r } function r(e, i, r) { var n = t(i || 1, r || 0); b.call(e, "src") !== n && h.call(e, "src", n) } function n(t, e) { t.naturalWidth ? e(t) : setTimeout(n, 100, t, e) } function c(t) { var c = i(t), o = t[l]; if (c["object-fit"] = c["object-fit"] || "fill", !o.img) { if ("fill" === c["object-fit"]) return; if (!o.skipTest && f && !c["object-position"]) return } if (!o.img) { o.img = new Image(t.width, t.height), o.img.srcset = b.call(t, "data-ofi-srcset") || t.srcset, o.img.src = b.call(t, "data-ofi-src") || t.src, h.call(t, "data-ofi-src", t.src), t.srcset && h.call(t, "data-ofi-srcset", t.srcset), r(t, t.naturalWidth || t.width, t.naturalHeight || t.height), t.srcset && (t.srcset = ""); try { s(t) } catch (t) { window.console && console.warn("https://bit.ly/ofi-old-browser") } } e(o.img), t.style.backgroundImage = 'url("' + (o.img.currentSrc || o.img.src).replace(/"/g, '\\"') + '")', t.style.backgroundPosition = c["object-position"] || "center", t.style.backgroundRepeat = "no-repeat", t.style.backgroundOrigin = "content-box", /scale-down/.test(c["object-fit"]) ? n(o.img, function () { o.img.naturalWidth > t.width || o.img.naturalHeight > t.height ? t.style.backgroundSize = "contain" : t.style.backgroundSize = "auto" }) : t.style.backgroundSize = c["object-fit"].replace("none", "auto").replace("fill", "100% 100%"), n(o.img, function (e) { r(t, e.naturalWidth, e.naturalHeight) }) } function s(t) { var e = { get: function (e) { return t[l].img[e ? e : "src"] }, set: function (e, i) { return t[l].img[i ? i : "src"] = e, h.call(t, "data-ofi-" + i, e), c(t), e } }; Object.defineProperty(t, "src", e), Object.defineProperty(t, "currentSrc", { get: function () { return e.get("currentSrc") } }), Object.defineProperty(t, "srcset", { get: function () { return e.get("srcset") }, set: function (t) { return e.set(t, "srcset") } }) } function o() { function t(t, e) { return t[l] && t[l].img && ("src" === e || "srcset" === e) ? t[l].img : t } d || (HTMLImageElement.prototype.getAttribute = function (e) { return b.call(t(this, e), e) }, HTMLImageElement.prototype.setAttribute = function (e, i) { return h.call(t(this, e), e, String(i)) }) } function a(t, e) { var i = !y && !t; if (e = e || {}, t = t || "img", d && !e.skipTest || !m) return !1; "img" === t ? t = document.getElementsByTagName("img") : "string" == typeof t ? t = document.querySelectorAll(t) : "length" in t || (t = [t]); for (var r = 0; r < t.length; r++)t[r][l] = t[r][l] || { skipTest: e.skipTest }, c(t[r]); i && (document.body.addEventListener("load", function (t) { "IMG" === t.target.tagName && a(t.target, { skipTest: e.skipTest }) }, !0), y = !0, t = "img"), e.watchMQ && window.addEventListener("resize", a.bind(null, t, { skipTest: e.skipTest })) } var l = "fregante:object-fit-images", u = /(object-fit|object-position)\s*:\s*([-.\w\s%]+)/g, g = "undefined" == typeof Image ? { style: { "object-position": 1 } } : new Image, f = "object-fit" in g.style, d = "object-position" in g.style, m = "background-size" in g.style, p = "string" == typeof g.currentSrc, b = g.getAttribute, h = g.setAttribute, y = !1; return a.supportsObjectFit = f, a.supportsObjectPosition = d, o(), a }();

        objectFitImages();
    }());

    /* eslint-enable */
}));
