import 'jquery';
import 'bootstrap/js/modal';
import Map from './Map';

export function init() {
  window.removeEventListener('popstate', handleHistoryPopstate);
  window.addEventListener('popstate', handleHistoryPopstate);
  showXtracurModalForShowImmediateXtracurEvent();
  registerClickHandlersForXtracurEvents();
}

function getXtracurModalParamsFromRow(xtracur_event_row, is_request_from_client) {
  var contents_url = xtracur_event_row.data('contents-url');
  var this_url = xtracur_event_row.data('this-url');
  var parent_url = xtracur_event_row.data('parent-url');
  return {
    'contents-url': contents_url,
    'this-url': this_url,
    'parent-url': parent_url,
    'is-request-from-client': is_request_from_client
  };
}

function registerClickHandlersForXtracurEvents() {
  // register click event handler to show modal for each xtracur event:
  $('.xtracur-event-row').unbind().on('click', function (e) {
    e.preventDefault();
    var xtracur_event_row = $(this);
    var xtracur_modal_params = getXtracurModalParamsFromRow(xtracur_event_row, true);
    showXtracurModal(xtracur_modal_params, false);
  });
};

function unregisterClickHandlersForXtracurEvents() {
  // unregister click event handler to prevent showing multiple modals:
  $('.xtracur-event-row').off('click');
};

function showXtracurModalForShowImmediateXtracurEvent() {
  // if user requested xtracur event modal by url, corresponding row
  // will have attr 'data-showimmediate' set to 'True' => show modal:
  var xtracur_event_row_to_show_immediate = $('.xtracur-event-row[data-show-immediate="True"]')
  if (xtracur_event_row_to_show_immediate.length) {
    var xtracur_modal_params = getXtracurModalParamsFromRow(xtracur_event_row_to_show_immediate, false);
    showXtracurModal(xtracur_modal_params, true);
  }
};

function xtracurModalParamsEquals(params1, params2) {
  return params1 != null && params2 != null
    && params1['contents-url'] == params2['contents-url']
    && params1['this-url'] == params2['this-url']
    && params1['parent-url'] == params2['parent-url']
    && params1['is-request-from-client'] == params2['is-request-from-client'];
}

function handleHistoryPopstate(e) {
  var xtracur_modal_params = window.history.state;
  if (!xtracur_modal_params) {
    hideXtracurModal();
  }
  else {
    showXtracurModal(xtracur_modal_params, false);
  }
}

function hideXtracurModal() {
  $('#xtracurModal').modal('hide');
}

var xtracurModalShowing = false;

function showXtracurModal(xtracur_modal_params, show_immediate) {
  var modal = $('#xtracurModal');
  if (!modal.length) {
    modal = $('<div id="xtracurModal" class="modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"></div>');
    $('body').append(modal);
  }
  if (!xtracurModalShowing) {
    xtracurModalShowing = true;
    unregisterClickHandlersForXtracurEvents();
    var contents_url = xtracur_modal_params['contents-url'];
    var this_url = xtracur_modal_params['this-url'];
    var parent_url = xtracur_modal_params['parent-url'];
    var is_request_from_client = xtracur_modal_params['is-request-from-client'];
    $.get(contents_url, function (data) {
      $('#xtracurModal').html(data);
      modal.on('show.bs.modal', function () {
        // user clicked on row => update browser url on show:
        if (is_request_from_client) {
          if (!xtracurModalParamsEquals(window.history.state, xtracur_modal_params)) {
            window.history.pushState(xtracur_modal_params, '', this_url);
          }
        }
        // todo: description
        else if (show_immediate) {
          window.history.replaceState(xtracur_modal_params, '', this_url);
        }
      });
      modal.on('hidden.bs.modal', function () {
        // user clicked on row => restore previous url on hide:
        if (is_request_from_client) {
          if (xtracurModalParamsEquals(window.history.state, xtracur_modal_params)) {
            window.history.back();
          }
        }
        // modal is requested by url from server => restore parent url on hide:
        else if (show_immediate) {
          window.history.replaceState(null, '', parent_url);
        }
        // register click events that were unregistered
        // in the beginning of the method:
        registerClickHandlersForXtracurEvents();
        // unbind modal events to prevent multiple calls:
        modal.off();
        xtracurModalShowing = false;
      });
      modal.modal('show');

      const mapContainer = document.getElementById('map-container');
      /* ie10 doesn't support .dataset prop */
      const latitude = mapContainer.getAttribute('data-lat');
      const longitude = mapContainer.getAttribute('data-lng');

      Map.render({ mapContainer, latitude, longitude });
    });
  }
}
