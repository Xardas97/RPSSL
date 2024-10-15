import ShapeImage from './ShapeImage';
import "./styles.css";

export default function ChoiceOption({ value, disable, onShapeClick }) {
  let clickableComponent = <div className="shape-choice-hover" onClick={onShapeClick}/>
  if (disable)
    clickableComponent = <></>

  return (
    <div className="shape-choice">
      {clickableComponent}
      <ShapeImage shape={value}/>
    </div>
  );
}