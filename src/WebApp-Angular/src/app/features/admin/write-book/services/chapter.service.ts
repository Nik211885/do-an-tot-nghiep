import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ChapterVersion } from "../chapter/chapter-editor/chapter-version/chapter-version.component";

@Injectable({
    providedIn: "root"
})
export class ChapterVersionService{
    constructor(httpClient: HttpClient){}
    public getChapterVersionByChaoterId(chapterId: string) : ChapterVersion[]{
        return [
            {
                id:"1",
                name: "Verison 1",
                createVersion:"Ninh",
                dateCreateVersion: new Date()
            },
            {
                id:"2",
                name: "Verison 2",
                createVersion:"Ninh",
                dateCreateVersion: new Date()
            },
            {
                id:"3",
                name: "Verison 3",
                createVersion:"Ninh",
                dateCreateVersion: new Date()
            }
        ]
    }
}