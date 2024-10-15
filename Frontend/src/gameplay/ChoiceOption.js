import ShapeImage from './ShapeImage';
import "./styles.css";

export default function ChoiceOption({ value, onShapeClick }) {
  return (
    <div className="shape-choice" onClick={onShapeClick}>
      <ShapeImage shape={value}/>
    </div>
  );
}