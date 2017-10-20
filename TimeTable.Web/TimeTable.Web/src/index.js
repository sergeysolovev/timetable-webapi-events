import './polyfills/promise';
import './polyfills/closest';
import './scripts/google.analytics.js';
import './scripts/google.tagmanager.js';
import { isIE9orBelow } from './utils/isIE';
import './index.less';
import './img/favicon.ico';

const hasAnyClasses = (...args) => args.some(
  cls => document.getElementsByClassName(cls).length
);

const hasAnyIds = (...args) => args.some(
  id => document.getElementById(id)
);

window.addEventListener('load', function () {
  try {
    initLocaleSwitcher();
    handleNavbarBackButton();
  } catch(error) {
    console.error(error);
  }

  if (hasAnyIds('week')) {
    import(/* webpackChunkName: 'datetimepicker' */ './components/DateTimePicker')
      .then(DateTimePicker => DateTimePicker.init())
      .catch(error => console.error(error));
  }

  // ie9 doesn't support pushstate
  // no need to load this chunk
  if (!isIE9orBelow() && hasAnyClasses('xtracur-event-row')) {
    import(/* webpackChunkName: 'xtracurmodal' */ './components/XtracurModal')
      .then(XtracurModal => XtracurModal.init())
      .catch(error => console.error(error));
  }

  if (hasAnyClasses('locations-educators-modal-btn', 'address-modal-btn')) {
    import(/* webpackChunkName: 'locationsmodals' */ './components/LocationsModals')
      .then(LocationsModals => {
        LocationsModals.initAddressModal();
        LocationsModals.initLocationsModal();
      })
      .catch(error => console.error(error));
  }

  if (document.querySelector('[data-toggle="collapse"]')) {
    import(/* webpackChunkName: 'collapse' */ './components/Collapse')
      .catch(error => console.error(error))
  }

  if (document.querySelector('[data-toggle="tooltip"]')) {
    import(/* webpackChunkName: 'tooltip' */ './components/Tooltip')
      .then(Tooltip => Tooltip.init())
      .catch(error => console.error(error));
  }
});

function handleNavbarBackButton() {
  const backButton = document.getElementById('navbar-back-button');
  if (backButton) {
    backButton.onclick = e => {
      e.preventDefault();
      history.back(-1);
    };
  }
}

function initLocaleSwitcher() {
  const submit = document.getElementById('locale-submit');
  const localeOptions = document.getElementsByClassName('locale-option');
  Array.prototype.forEach.call(localeOptions, locale => {
    if (locale.checked) {
      const controlGroupElement = locale.closest('.control-group');
      if (controlGroupElement) {
        /* ie9 support */
        controlGroupElement.className += ' active';
      }
    }
    if (submit) {
      locale.onclick = e => {
        submit.click();
      };
    }
  });
}
