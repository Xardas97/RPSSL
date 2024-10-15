import ChoiceOption from './ChoiceOption';
import "./styles.css";

export default function ChoiceBoard({shapes, disable, onPlayShape}) {
  async function handleClick(shape) {
    await onPlayShape(shape)
  }

  const choiceButtons = shapes.map(choice =>
    <ChoiceOption key={choice.id} value={choice.name} disable={disable} onShapeClick={async () => await handleClick(choice)} />
  )

  return (
    <div className="choice-board">
      {choiceButtons}
    </div>
  );
}