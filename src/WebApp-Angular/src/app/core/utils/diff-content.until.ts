import { DiffDOM } from "diff-dom";


function parseHtml(html: string): HTMLElement {
  const wrapper = document.createElement('div');
  wrapper.innerHTML = html;
  return wrapper;
}

export function getDelta(oldHtml: string, newHtml: string): string{
  const dd = new DiffDOM();
  const oldNode = parseHtml(oldHtml);
  const newNode = parseHtml(newHtml);
  const delta = dd.diff(oldNode, newNode);
  return JSON.stringify(delta);
}

export function applyDelta(html: string, delta: any[]): string {
  const node = parseHtml(html);
  const dd = new DiffDOM();
  dd.apply(node, delta);
  return node.innerHTML;
}

export function undoDiff(newHtml: string, delta: any[]): string {
  const node = parseHtml(newHtml);
  const dd = new DiffDOM();
  dd.undo(node, delta);
  return node.innerHTML;
}

export function applyDeltaJson(html: string, deltaJson: string) : string {
  const delta = JSON.parse(deltaJson);
  return applyDelta(html, delta);
}

export function undoDiffJson(html: string, deltaJson: string) : string {
  const delta = JSON.parse(deltaJson);
  return undoDiff(html, delta);
}
