import Loader from './Loader';
import getCookie from '../utils/getCookie';
import ymaps from 'ymaps';

import './Map.less';

const Map = ({
  render({mapContainer, latitude, longitude} = {}) {
    if (!mapContainer || !latitude || !longitude) {
      mapContainer.style.display = 'none';
      return;
    }
    mapContainer.className = 'map-container';
    const apiEndpoint = getApiEndpoint();
    const loader = Loader();
    mapContainer.appendChild(loader);
    ymaps.load(apiEndpoint)
      .then(ym => {
        /* ie11 doesn't support loader.remove() */
        mapContainer.removeChild(loader);
        const map = new ym.Map(mapContainer.id, {
          center: [latitude, longitude],
          zoom: 16,
          controls: ['default', 'routeEditor']
        });
        var marker = new ym.Placemark([latitude, longitude]);
        map.geoObjects.add(marker);
        map.behaviors.disable('scrollZoom');
      })
      .catch(() => mapContainer.style.display = 'none');
  }
});

const getApiEndpoint = () => {
  const getLang = () => {
    // try to determine the language from the cookie
    let lang = getCookie('_culture');
    if (!lang) {
      // try to determine the language from the browser
      const lang = window.navigator.userLanguage || window.navigator.language;
    }
    /* use indexOf to support ie */
    if (lang && lang.indexOf('ru') !== -1) {
      return 'ru';
    }
    if (lang && lang.indexOf('en') !== -1) {
      return 'en-RU';
    }
    // if nothing has been found
    // use default language 'en-RU'
    return 'en-RU';
  };

  return `//api-maps.yandex.ru/2.1.53/?lang=${getLang()}`;
}

export default Map;
