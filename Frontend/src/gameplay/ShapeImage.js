import "./styles.css";

export default function ShapeImage({shape}) {
  let imageSource;
  try {
    imageSource = require('./images/' + shape + '.png')
    return ( <img className="shape-image" src={imageSource}/> )
  } catch (ex) {
    imageSource = require('./images/NotSupported.png')
    return ( <img className="missing-shape-image" src={imageSource}/> )
  }
}
