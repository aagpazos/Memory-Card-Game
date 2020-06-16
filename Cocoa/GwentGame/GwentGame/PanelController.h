//
//  PanelController.h
//  GwentGame
//
//  Created by Adrian on 18/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import <Cocoa/Cocoa.h>
@class Baraja;

NS_ASSUME_NONNULL_BEGIN

@interface PanelController : NSWindowController
    <NSTabViewDelegate, NSTableViewDelegate>{
        IBOutlet NSTableView *aTableView;
        IBOutlet NSButton *buttonDelete;
        IBOutlet NSButton *infoButton;
        IBOutlet NSTextField *textField;
        Baraja *baraja;
}

@property Baraja *baraja;

-(IBAction)buttonDelete:(id)sender;
-(IBAction)sliderAction:(id)sender;
-(IBAction)info:(id)sender;
-(id)initWithBaraja:(Baraja *)baraja;
-(void)reloadTableData;

@end

NS_ASSUME_NONNULL_END
