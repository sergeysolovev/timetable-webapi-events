import * as ie from '../utils/isIE';
import './Loader.less';

const Loader = ({size = 64} = {}) => {
  /* ie9 is not supported, return empty div */
  if (ie.isIE9orBelow()) {
    return document.createElement('div');
  }

  const loader = document.createElement('div');
  loader.className = 'loader';
  loader.style.width = `${size}px`;

  const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
  svg.setAttribute('class', 'circular');
  svg.setAttribute('viewBox', '25 25 50 50');

  const circle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
  circle.setAttribute('class', 'path');
  circle.setAttribute('cx', '50');
  circle.setAttribute('cy', '50');
  circle.setAttribute('r', '20');
  circle.setAttribute('fill', 'none');
  circle.setAttribute('stroke-width', '2');
  circle.setAttribute('stroke-miterlimit', '10');
  /* for ie10 & ie11 animation is supported
     but not as good as for normal browsers */
  if (ie.isIE10() || ie.isIE11orAbove()) {
    circle.setAttribute('stroke', '#E95420');
    circle.setAttribute('stroke-dasharray', '89, 200');
  } else {
    circle.setAttribute('stroke-dasharray', '1, 200');
  }

  loader
    .appendChild(svg)
    .appendChild(circle);

  return loader;
}

export default Loader;
