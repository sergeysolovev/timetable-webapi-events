import 'jquery';
import 'bootstrap/js/modal';
import Map from './Map';

export function initAddressModal() {
  var createModal = function (name, address) {
    var id = 'map-address-modal';
    var modal = $('<div class="modal" id="' + id + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button><h4 class="modal-title" id="myModalLabel">&nbsp;</h4></div><div class="modal-body"><div id="map-container"></div></div><div class="modal-footer"></div></div></div></div>');
    $('body').append(modal);
    $('#' + id + ' .modal-title').html(name);
    $('#' + id + ' .modal-footer').html(address);
    return modal;
  };

  $('.address-modal-btn').unbind().on('click', function (e) {
    e.preventDefault();

    var $this = $(this);
    var lat = $this.attr('data-lat').replace(',', '.');
    var lng = $this.attr('data-lng').replace(',', '.');
    var name = $this.data('name');
    var address = $this.data('address');

    if (lat != '' && lng != '') {
      var modal = createModal(name, address);
      modal.modal();
      modal.on('hidden.bs.modal', function () {
        $(this).remove();
      });

      Map.render({
        mapContainer: document.getElementById('map-container'),
        latitude: lat,
        longitude: lng
      });
    }
  });
}

export function initLocationsModal() {
  var createModal = function (name, time, content) {
    var id = 'locations-educators-modal';
    var modal = $('<div class="modal" id="' + id + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button><h4 class="modal-title" id="myModalLabel">&nbsp;</h4></div><div class="modal-body nopadding"></div><div class="modal-footer"></div></div></div></div>');
    $('body').append(modal);
    $('#' + id + ' .modal-title').html(name);
    $('#' + id + ' .modal-body').append(content);
    $('#' + id + ' .modal-footer').html(time);
    $('[data-toggle="tooltip"]').tooltip();

    return modal;
  };

  var elems = $('.locations-educators-modal-btn');

  elems.on('click', function (e) {
    e.preventDefault();

    var $this = $(this);
    var name = $this.data('name');
    var time = $this.data('time');
    var table = $this.parent().parent().parent().find('.studyevent-location-educator-modal .location-educator-table');


    if (table.length > 0) {
      var modal = createModal(name, time, table.clone());

      modal.modal();
      modal.on('hidden.bs.modal', function () {
        $(this).remove();
      });

      setTimeout(() => initAddressModal(), 500);
    }
  });
}
